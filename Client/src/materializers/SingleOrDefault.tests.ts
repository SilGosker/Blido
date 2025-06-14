import { singleOrDefault } from "./SingleOrDefault";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartCursorModule from "../StartCursor";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartCursor", () => ({
    startCursor: jest.fn()
}));

describe("singleOrDefault(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it("should resolve with the single matching record", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(false)  // First record doesn't match
            .mockReturnValueOnce(true)   // Second record matches
            .mockReturnValueOnce(false); // Third record doesn't match

        const mockRecord1 = { id: 1, value: 10 };
        const mockRecord2 = { id: 2, value: 20 }; // This should be returned
        const mockRecord3 = { id: 3, value: 30 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: jest.fn()
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
        const promise = singleOrDefault(mockJson);

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
        await expect(promise).resolves.toBe(mockRecord2);
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord3);
    });

    it("should reject when multiple records match the criteria", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(true)   // First record matches
            .mockReturnValueOnce(true);  // Second record also matches

        const mockRecord1 = { id: 1, value: 10 };
        const mockRecord2 = { id: 2, value: 20 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: jest.fn()
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
        const promise = singleOrDefault(mockJson);

        // Simulate first match
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate second match (should cause rejection)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).rejects.toBe('More than one element satisfies the condition in predicate');
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
    });

    it("should resolve with null when no records match the criteria", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(false);

        const mockRecord = { id: 1, value: 42 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: jest.fn()
        });

        let successCallback: EventListener | null = null;

        const mockCursor = {
            value: mockRecord,
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown;
            addEventListener: (name: string, callback: EventListener) => void;
        } = {
            result: mockCursor,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as unknown as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = singleOrDefault(mockJson);

        // Simulate success event (no match)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // End of cursor
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBeNull();
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord);
    });

    it("should resolve with null when there are no records in the database", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn();

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: jest.fn()
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
        const promise = singleOrDefault(mockJson);

        // Simulate success event with no records
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBeNull();
        expect(mockMatchesFn).not.toHaveBeenCalled();
    });

    it("should reject when an error occurs", async () => {
        // Arrange
        const mockJson = "{}";
        const mockError = new Error("Database error");

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: jest.fn(),
            selector: jest.fn()
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
        const promise = singleOrDefault(mockJson);

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

    it("should handle first record match correctly", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(true)    // First record matches
            .mockReturnValueOnce(false);  // Second record doesn't match

        const mockRecord1 = { id: 1, value: 100 }; // This should be returned
        const mockRecord2 = { id: 2, value: 200 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: jest.fn()
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
        const promise = singleOrDefault(mockJson);

        // Simulate first match
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate second record (no match)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // End of cursor
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
    });

    it("should handle case where truthy result is already found when checking for multiple matches", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(true)   // First record matches (truthy object)
            .mockReturnValueOnce(true);  // Second record also matches

        const mockRecord1 = { id: 1, value: 10, flag: true }; // Truthy object
        const mockRecord2 = { id: 2, value: 20 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: jest.fn()
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
        const promise = singleOrDefault(mockJson);

        // Simulate first match
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate second match (should cause rejection)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).rejects.toBe('More than one element satisfies the condition in predicate');
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
    });

    it("should correctly handle a falsy matching record", async () => {
        // Arrange - testing edge case where the matching record itself is falsy
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(true)    // First record matches (but is falsy)
            .mockReturnValueOnce(false);  // Second record doesn't match

        const mockRecord1 = null; // Falsy but valid record
        const mockRecord2 = { id: 2, value: 200 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: jest.fn()
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
        const promise = singleOrDefault(mockJson);

        // Simulate first match (falsy record)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate second record (no match)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // End of cursor
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert - should return the falsy record
        await expect(promise).resolves.toBeNull();
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
    });
});