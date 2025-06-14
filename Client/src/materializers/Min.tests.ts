import { min } from "./Min";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartCursorModule from "../StartCursor";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartCursor", () => ({
    startCursor: jest.fn()
}));

describe("min(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it("should resolve with the minimum value when only one record matches", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockSelectorFn = jest.fn().mockReturnValue(42);
        const mockRecord = { id: 1, value: 42 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: mockSelectorFn
        });

        let successCallback: EventListener | null = null;

        const mockCursor = {
            value: mockRecord,
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown;
            addEventListener: (name: string, callback: EventListener) => void;
        }  = {
            result: mockCursor,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as unknown as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = min(mockJson);

        // Simulate first success event (match found)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate end of cursor (no more records)
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(42);
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord);
        expect(mockCursor.continue).toHaveBeenCalled(); // Should continue to find potential lower values
    });

    it("should resolve with the minimum value when multiple records match", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(50)  // First record value
            .mockReturnValueOnce(10)  // Second record value (lower)
            .mockReturnValueOnce(25); // Third record value (higher than min)

        const mockRecord1 = { id: 1, value: 50 };
        const mockRecord2 = { id: 2, value: 10 };
        const mockRecord3 = { id: 3, value: 25 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: mockSelectorFn
        });

        let successCallback: EventListener | null = null;

        const mockCursor1 = {
            value: mockRecord1,
            continue: jest.fn()
        };

        const mockCursor2 = {
            value: mockRecord2,
            continue: jest.fn()
        };

        const mockCursor3 = {
            value: mockRecord3,
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown;
            addEventListener: (name: string, callback: EventListener) => void;
        }  = {
            result: mockCursor1,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as unknown as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = min(mockJson);

        // Simulate first success event (first value: 50)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate second success event (lower value: 10)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate third success event (higher value: 25)
        mockRequest.result = mockCursor3;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate end of cursor
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(10);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord3);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord3);
    });

    it("should handle negative numbers correctly", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(-5)   // First record value
            .mockReturnValueOnce(-10); // Second record value (lower)

        const mockRecord1 = { id: 1, value: -5 };
        const mockRecord2 = { id: 2, value: -10 };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: mockSelectorFn
        });

        let successCallback: EventListener | null = null;

        const mockCursor1 = {
            value: mockRecord1,
            continue: jest.fn()
        };

        const mockCursor2 = {
            value: mockRecord2,
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown;
            addEventListener: (name: string, callback: EventListener) => void;
        }  = {
            result: mockCursor1,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as unknown as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = min(mockJson);

        // Simulate first success event (-5)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate second success event (-10, which is lower)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate end of cursor
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(-10);
    });

    it("should skip records that don't match the criteria", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(false)  // First record doesn't match
            .mockReturnValueOnce(true)   // Second record matches
            .mockReturnValueOnce(false)  // Third record doesn't match
            .mockReturnValueOnce(true);  // Fourth record matches

        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(30)  // Second record value
            .mockReturnValueOnce(20); // Fourth record value (lower)

        const mockRecord1 = { id: 1, value: 5 };   // Won't match
        const mockRecord2 = { id: 2, value: 30 };  // Matches
        const mockRecord3 = { id: 3, value: 1 };   // Won't match
        const mockRecord4 = { id: 4, value: 20 };  // Matches

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: mockSelectorFn
        });

        let successCallback: EventListener | null = null;

        const mockCursor1 = { value: mockRecord1, continue: jest.fn() };
        const mockCursor2 = { value: mockRecord2, continue: jest.fn() };
        const mockCursor3 = { value: mockRecord3, continue: jest.fn() };
        const mockCursor4 = { value: mockRecord4, continue: jest.fn() };

        const mockRequest: {
            result: unknown;
            addEventListener: (name: string, callback: EventListener) => void;
        }  = {
            result: mockCursor1,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as unknown as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = min(mockJson);

        // Simulate events
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = mockCursor3;
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = mockCursor4;
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(20);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord4);
        expect(mockSelectorFn).not.toHaveBeenCalledWith(mockRecord1);
        expect(mockSelectorFn).not.toHaveBeenCalledWith(mockRecord3);
    });

    it("should reject when no records match criteria", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(false);
        const mockSelectorFn = jest.fn();

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: mockSelectorFn
        });

        let successCallback: EventListener | null = null;

        const mockCursor = {
            value: { id: 1, value: 42 },
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
        const promise = min(mockJson);

        // Simulate first success event (no match)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Update cursor to null to indicate end of records
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).rejects.toBe('Source contains no elements.');
        expect(mockSelectorFn).not.toHaveBeenCalled();
    });

    it("should reject when there are no records", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn();
        const mockSelectorFn = jest.fn();

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: mockSelectorFn
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
        const promise = min(mockJson);

        // Simulate success event with no records
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).rejects.toBe('Source contains no elements.');
        expect(mockMatchesFn).not.toHaveBeenCalled();
        expect(mockSelectorFn).not.toHaveBeenCalled();
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
        const promise = min(mockJson);

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