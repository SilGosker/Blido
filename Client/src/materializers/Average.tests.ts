import { average } from "./Average";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartCursorModule from "../StartCursor";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartCursor", () => ({
    startCursor: jest.fn()
}));

describe("average(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it("should calculate the average of matching records", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(10)
            .mockReturnValueOnce(20)
            .mockReturnValueOnce(30);

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: mockSelectorFn
        });

        let successCallback: EventListener | null = null;

        // Create mock cursors
        const mockCursor1 = {
            value: { id: 1, value: 10 },
            continue: jest.fn()
        };

        const mockCursor2 = {
            value: { id: 2, value: 20 },
            continue: jest.fn()
        };

        const mockCursor3 = {
            value: { id: 3, value: 30 },
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown,
            addEventListener: (event: string, callback: EventListener) => void
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
        const promise = average(mockJson);

        // Simulate cursor iterations
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = mockCursor3;
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = null; // End of records
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(20); // (10 + 20 + 30) / 3 = 20
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledTimes(3);
        expect(mockSelectorFn).toHaveBeenCalledTimes(3);
        expect(mockCursor1.continue).toHaveBeenCalled();
        expect(mockCursor2.continue).toHaveBeenCalled();
        expect(mockCursor3.continue).toHaveBeenCalled();
    });

    it("should only include values from records that match the criteria", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(true)   // First record matches
            .mockReturnValueOnce(false)  // Second record doesn't match
            .mockReturnValueOnce(true);  // Third record matches

        const mockSelectorFn = jest.fn()
            .mockReturnValueOnce(10)
            .mockReturnValueOnce(30);

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn,
            selector: mockSelectorFn
        });

        let successCallback: EventListener | null = null;

        // Create mock cursors
        const mockCursor1 = {
            value: { id: 1, value: 10 },
            continue: jest.fn()
        };

        const mockCursor2 = {
            value: { id: 2, value: 20 },
            continue: jest.fn()
        };

        const mockCursor3 = {
            value: { id: 3, value: 30 },
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown,
            addEventListener: (event: string, callback: EventListener) => void
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
        const promise = average(mockJson);

        // Simulate cursor iterations
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = mockCursor3;
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = null; // End of records
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(20); // (10 + 30) / 2 = 20
        expect(mockMatchesFn).toHaveBeenCalledTimes(3);
        expect(mockSelectorFn).toHaveBeenCalledTimes(2); // Only called for matching records
        expect(mockCursor1.continue).toHaveBeenCalled();
        expect(mockCursor2.continue).toHaveBeenCalled();
        expect(mockCursor3.continue).toHaveBeenCalled();
    });

    it("should handle case with no matching records", async () => {
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
            value: { id: 1, value: 10 },
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown,
            addEventListener: (event: string, callback: EventListener) => void
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
        const promise = average(mockJson);

        // Simulate cursor iterations
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = null; // End of records
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBeNaN(); // 0/0 = NaN
        expect(mockMatchesFn).toHaveBeenCalledTimes(1);
        expect(mockSelectorFn).not.toHaveBeenCalled(); // Never called as no records matched
        expect(mockCursor.continue).toHaveBeenCalled();
    });

    it("should handle case with no records", async () => {
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
        const promise = average(mockJson);

        // Simulate success event with no records
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBeNaN(); // 0/0 = NaN
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
        const promise = average(mockJson);

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