import { toArray } from "./ToArray";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartTransactionModule from "../StartTransaction";
import * as StartCursorModule from "../StartCursor";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartTransaction", () => ({
    startTransaction: jest.fn()
}));

jest.mock("../StartCursor", () => ({
    startCursor: jest.fn()
}));

describe("toArray(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    describe("when hasFilters is false", () => {
        it("should resolve with all records using getAll", async () => {
            // Arrange
            const mockJson = "{}";
            const mockRecords = [
                { id: 1, name: "John" },
                { id: 2, name: "Jane" },
                { id: 3, name: "Bob" }
            ];

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: false
            });

            const mockRequest: {
                result: unknown[],
                onsuccess: null | (() => void),
                onerror: null | (() => void)
            }  = {
                result: mockRecords,
                onsuccess: null,
                onerror: null
            };

            const mockTransaction = {
                getAll: jest.fn().mockReturnValue(mockRequest)
            };

            (StartTransactionModule.startTransaction as jest.Mock).mockResolvedValue(mockTransaction);

            // Act
            const promise = toArray(mockJson);

            // Simulate success callback
            await Promise.resolve();
            if (mockRequest.onsuccess) {
                mockRequest.onsuccess();
            }

            // Assert
            await expect(promise).resolves.toEqual(mockRecords);
            expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
            expect(StartTransactionModule.startTransaction).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
            expect(mockTransaction.getAll).toHaveBeenCalled();
        });

        it("should reject when getAll request fails", async () => {
            // Arrange
            const mockJson = "{}";

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: false
            });

            const mockRequest: {
                result: null,
                onsuccess: null | (() => void),
                onerror: null | (() => void)
            } = {
                result: null,
                onsuccess: null,
                onerror: null
            };

            const mockTransaction = {
                getAll: jest.fn().mockReturnValue(mockRequest)
            };

            (StartTransactionModule.startTransaction as jest.Mock).mockResolvedValue(mockTransaction);

            // Act
            const promise = toArray(mockJson);

            // Simulate error callback
            await Promise.resolve();
            if (mockRequest.onerror) {
                mockRequest.onerror();
            }

            // Assert
            await expect(promise).rejects.toBeUndefined();
        });

        it("should resolve with empty array when no records exist", async () => {
            // Arrange
            const mockJson = "{}";
            const mockRecords: unknown[] = [];

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: false
            });

            const mockRequest: {
                result: unknown[],
                onsuccess: null | (() => void),
                onerror: null | (() => void)
            } = {
                result: mockRecords,
                onsuccess: null,
                onerror: null
            };

            const mockTransaction = {
                getAll: jest.fn().mockReturnValue(mockRequest)
            };

            (StartTransactionModule.startTransaction as jest.Mock).mockResolvedValue(mockTransaction);

            // Act
            const promise = toArray(mockJson);

            // Simulate success callback
            await Promise.resolve();
            if (mockRequest.onsuccess) {
                mockRequest.onsuccess();
            }

            // Assert
            await expect(promise).resolves.toEqual([]);
        });
    });

    describe("when hasFilters is true", () => {
        it("should resolve with filtered records when multiple records match", async () => {
            // Arrange
            const mockJson = "{}";
            const mockMatchesFn = jest.fn()
                .mockReturnValueOnce(true)   // First record matches
                .mockReturnValueOnce(false)  // Second record doesn't match
                .mockReturnValueOnce(true);  // Third record matches

            const mockRecord1 = { id: 1, name: "John", active: true };
            const mockRecord2 = { id: 2, name: "Jane", active: false };
            const mockRecord3 = { id: 3, name: "Bob", active: true };

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: true,
                matches: mockMatchesFn
            });

            let successCallback: EventListener | null = null;

            const mockCursor1 = { value: mockRecord1, continue: jest.fn() };
            const mockCursor2 = { value: mockRecord2, continue: jest.fn() };
            const mockCursor3 = { value: mockRecord3, continue: jest.fn() };

            const mockRequest: {
                result: unknown;
                addEventListener: (name: string, callback: EventListener) => void;
            } = {
                result: mockCursor1,
                addEventListener: jest.fn((event, callback) => {
                    if (event === 'success') {
                        successCallback = callback as unknown as EventListener;
                    }
                })
            };

            (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

            // Act
            const promise = toArray(mockJson);

            // Simulate events
            await Promise.resolve();
            (successCallback as unknown as EventListener)(new Event("success"));

            mockRequest.result = mockCursor2;
            (successCallback as unknown as EventListener)(new Event("success"));

            mockRequest.result = mockCursor3;
            (successCallback as unknown as EventListener)(new Event("success"));

            // End of cursor
            mockRequest.result = null;
            (successCallback as unknown as EventListener)(new Event("success"));

            // Assert
            await expect(promise).resolves.toEqual([mockRecord1, mockRecord3]);
            expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
            expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord3);
        });

        it("should resolve with single record when only one record matches", async () => {
            // Arrange
            const mockJson = "{}";
            const mockMatchesFn = jest.fn()
                .mockReturnValueOnce(false)  // First record doesn't match
                .mockReturnValueOnce(true);  // Second record matches

            const mockRecord1 = { id: 1, name: "John", active: false };
            const mockRecord2 = { id: 2, name: "Jane", active: true };

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: true,
                matches: mockMatchesFn
            });

            let successCallback: EventListener | null = null;

            const mockCursor1 = { value: mockRecord1, continue: jest.fn() };
            const mockCursor2 = { value: mockRecord2, continue: jest.fn() };

            const mockRequest: {
                result: unknown;
                addEventListener: (name: string, callback: EventListener) => void;
            } = {
                result: mockCursor1,
                addEventListener: jest.fn((event, callback) => {
                    if (event === 'success') {
                        successCallback = callback as unknown as EventListener;
                    }
                })
            };

            (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

            // Act
            const promise = toArray(mockJson);

            // Simulate events
            await Promise.resolve();
            (successCallback as unknown as EventListener)(new Event("success"));

            mockRequest.result = mockCursor2;
            (successCallback as unknown as EventListener)(new Event("success"));

            // End of cursor
            mockRequest.result = null;
            (successCallback as unknown as EventListener)(new Event("success"));

            // Assert
            await expect(promise).resolves.toEqual([mockRecord2]);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
        });

        it("should resolve with empty array when no records match criteria", async () => {
            // Arrange
            const mockJson = "{}";
            const mockMatchesFn = jest.fn().mockReturnValue(false);

            const mockRecord1 = { id: 1, name: "John", active: false };
            const mockRecord2 = { id: 2, name: "Jane", active: false };

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: true,
                matches: mockMatchesFn
            });

            let successCallback: EventListener | null = null;

            const mockCursor1 = { value: mockRecord1, continue: jest.fn() };
            const mockCursor2 = { value: mockRecord2, continue: jest.fn() };

            const mockRequest: {
                result: unknown;
                addEventListener: (name: string, callback: EventListener) => void;
            } = {
                result: mockCursor1,
                addEventListener: jest.fn((event, callback) => {
                    if (event === 'success') {
                        successCallback = callback as unknown as EventListener;
                    }
                })
            };

            (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

            // Act
            const promise = toArray(mockJson);

            // Simulate events
            await Promise.resolve();
            (successCallback as unknown as EventListener)(new Event("success"));

            mockRequest.result = mockCursor2;
            (successCallback as unknown as EventListener)(new Event("success"));

            // End of cursor
            mockRequest.result = null;
            (successCallback as unknown as EventListener)(new Event("success"));

            // Assert
            await expect(promise).resolves.toEqual([]);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
        });

        it("should resolve with empty array when there are no records", async () => {
            // Arrange
            const mockJson = "{}";
            const mockMatchesFn = jest.fn();

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: true,
                matches: mockMatchesFn
            });

            let successCallback: EventListener | null = null;

            const mockRequest = {
                result: null, // No cursor means no records
                addEventListener: jest.fn((event, callback) => {
                    if (event === 'success') {
                        successCallback = callback as unknown as EventListener;
                    }
                })
            };

            (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

            // Act
            const promise = toArray(mockJson);

            // Simulate success event with no records
            await Promise.resolve();
            (successCallback as unknown as EventListener)(new Event("success"));

            // Assert
            await expect(promise).resolves.toEqual([]);
            expect(mockMatchesFn).not.toHaveBeenCalled();
        });

        it("should handle all records matching", async () => {
            // Arrange
            const mockJson = "{}";
            const mockMatchesFn = jest.fn().mockReturnValue(true);

            const mockRecord1 = { id: 1, name: "John", active: true };
            const mockRecord2 = { id: 2, name: "Jane", active: true };
            const mockRecord3 = { id: 3, name: "Bob", active: true };

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: true,
                matches: mockMatchesFn
            });

            let successCallback: EventListener | null = null;

            const mockCursor1 = { value: mockRecord1, continue: jest.fn() };
            const mockCursor2 = { value: mockRecord2, continue: jest.fn() };
            const mockCursor3 = { value: mockRecord3, continue: jest.fn() };

            const mockRequest: {
                result: unknown;
                addEventListener: (name: string, callback: EventListener) => void;
            } = {
                result: mockCursor1,
                addEventListener: jest.fn((event, callback) => {
                    if (event === 'success') {
                        successCallback = callback as unknown as EventListener;
                    }
                })
            };

            (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

            // Act
            const promise = toArray(mockJson);

            // Simulate events
            await Promise.resolve();
            (successCallback as unknown as EventListener)(new Event("success"));

            mockRequest.result = mockCursor2;
            (successCallback as unknown as EventListener)(new Event("success"));

            mockRequest.result = mockCursor3;
            (successCallback as unknown as EventListener)(new Event("success"));

            // End of cursor
            mockRequest.result = null;
            (successCallback as unknown as EventListener)(new Event("success"));

            // Assert
            await expect(promise).resolves.toEqual([mockRecord1, mockRecord2, mockRecord3]);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
            expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord3);
        });

        it("should handle complex objects correctly", async () => {
            // Arrange
            const mockJson = "{}";
            const mockMatchesFn = jest.fn().mockReturnValue(true);

            const mockRecord1 = {
                id: 1,
                user: { name: "John", age: 30 },
                tags: ["developer", "javascript"],
                metadata: { created: "2023-01-01", updated: null }
            };
            const mockRecord2 = {
                id: 2,
                user: { name: "Jane", age: 25 },
                tags: ["designer", "css"],
                metadata: { created: "2023-02-01", updated: "2023-03-01" }
            };

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: true,
                matches: mockMatchesFn
            });

            let successCallback: EventListener | null = null;

            const mockCursor1 = { value: mockRecord1, continue: jest.fn() };
            const mockCursor2 = { value: mockRecord2, continue: jest.fn() };

            const mockRequest: {
                result: unknown;
                addEventListener: (name: string, callback: EventListener) => void;
            } = {
                result: mockCursor1,
                addEventListener: jest.fn((event, callback) => {
                    if (event === 'success') {
                        successCallback = callback as unknown as EventListener;
                    }
                })
            };

            (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

            // Act
            const promise = toArray(mockJson);

            // Simulate events
            await Promise.resolve();
            (successCallback as unknown as EventListener)(new Event("success"));

            mockRequest.result = mockCursor2;
            (successCallback as unknown as EventListener)(new Event("success"));

            // End of cursor
            mockRequest.result = null;
            (successCallback as unknown as EventListener)(new Event("success"));

            // Assert
            await expect(promise).resolves.toEqual([mockRecord1, mockRecord2]);
        });

        it("should reject when an error occurs", async () => {
            // Arrange
            const mockJson = "{}";
            const mockError = new Error("Database error");

            (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
                databaseName: 'testDB',
                currentVersion: 1,
                objectStoreName: 'testObjectStore',
                hasFilters: true,
                matches: jest.fn()
            });

            let errorCallback: EventListener | null = null;

            const mockRequest = {
                addEventListener: jest.fn((event, callback) => {
                    if (event === 'error') {
                        errorCallback = callback as unknown as EventListener;
                    }
                })
            };

            (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

            // Act
            const promise = toArray(mockJson);

            // Create a custom error event
            const errorEvent = new Event("error");
            Object.defineProperty(errorEvent, 'target', {
                value: { error: mockError },
                enumerable: true
            });

            // Simulate error event
            await Promise.resolve();
            (errorCallback as unknown as EventListener)(errorEvent);

            // Assert
            await expect(promise).rejects.toEqual(errorEvent);
            expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
            expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        });
    });
});