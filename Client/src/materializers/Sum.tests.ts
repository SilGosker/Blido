import { sum } from "./Sum";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartCursorModule from "../StartCursor";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartCursor", () => ({
    startCursor: jest.fn()
}));

describe("sum(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it("should resolve with sum of selected values when multiple records match", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(10)  // First record value
            .mockReturnValueOnce(25)  // Second record value
            .mockReturnValueOnce(15); // Third record value

        const mockRecord1 = { id: 1, value: 10 };
        const mockRecord2 = { id: 2, value: 25 };
        const mockRecord3 = { id: 3, value: 15 };

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
        const promise = sum(mockJson);

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
        await expect(promise).resolves.toBe(50); // 10 + 25 + 15 = 50
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord3);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord3);
    });

    it("should resolve with sum when only one record matches", async () => {
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

        const mockCursor = { value: mockRecord, continue: jest.fn() };

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
        const promise = sum(mockJson);

        // Simulate success event
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // End of cursor
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(42);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord);
    });

    it("should handle negative numbers correctly", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(10)   // First record value
            .mockReturnValueOnce(-5)   // Second record value (negative)
            .mockReturnValueOnce(-3);  // Third record value (negative)

        const mockRecord1 = { id: 1, value: 10 };
        const mockRecord2 = { id: 2, value: -5 };
        const mockRecord3 = { id: 3, value: -3 };

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
        const promise = sum(mockJson);

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
        await expect(promise).resolves.toBe(2); // 10 + (-5) + (-3) = 2
    });

    it("should handle decimal numbers correctly", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(10.5)   // First record value
            .mockReturnValueOnce(5.25)   // Second record value
            .mockReturnValueOnce(2.75);  // Third record value

        const mockRecord1 = { id: 1, value: 10.5 };
        const mockRecord2 = { id: 2, value: 5.25 };
        const mockRecord3 = { id: 3, value: 2.75 };

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
        const promise = sum(mockJson);

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
        await expect(promise).resolves.toBe(18.5); // 10.5 + 5.25 + 2.75 = 18.5
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
            .mockReturnValueOnce(20)  // Second record value
            .mockReturnValueOnce(30); // Fourth record value

        const mockRecord1 = { id: 1, value: 100 }; // Won't match
        const mockRecord2 = { id: 2, value: 20 };  // Matches
        const mockRecord3 = { id: 3, value: 200 }; // Won't match
        const mockRecord4 = { id: 4, value: 30 };  // Matches

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
        const promise = sum(mockJson);

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
        await expect(promise).resolves.toBe(50); // 20 + 30 = 50
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockSelectorFn).toHaveBeenCalledWith(mockRecord4);
        expect(mockSelectorFn).not.toHaveBeenCalledWith(mockRecord1);
        expect(mockSelectorFn).not.toHaveBeenCalledWith(mockRecord3);
    });

    it("should resolve with 0 when no records match criteria", async () => {
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
        const promise = sum(mockJson);

        // Simulate success event (no match)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // End of cursor
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(0);
        expect(mockSelectorFn).not.toHaveBeenCalled();
    });

    it("should resolve with 0 when there are no records", async () => {
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
        const promise = sum(mockJson);

        // Simulate success event with no records
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(0);
        expect(mockMatchesFn).not.toHaveBeenCalled();
        expect(mockSelectorFn).not.toHaveBeenCalled();
    });

    it("should handle zero values correctly", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(5)   // First record value
            .mockReturnValueOnce(0)   // Second record value (zero)
            .mockReturnValueOnce(3);  // Third record value

        const mockRecord1 = { id: 1, value: 5 };
        const mockRecord2 = { id: 2, value: 0 };
        const mockRecord3 = { id: 3, value: 3 };

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
        const promise = sum(mockJson);

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
        await expect(promise).resolves.toBe(8); // 5 + 0 + 3 = 8
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
        const promise = sum(mockJson);

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