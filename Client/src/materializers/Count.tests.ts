import { count } from "./Count";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartCursorModule from "../StartCursor";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartCursor", () => ({
    startCursor: jest.fn()
}));

describe("count(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it("should count all matching records", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn
        });

        let successCallback: EventListener | null = null;

        // Create mock cursors
        const mockCursor1 = {
            value: { id: 1, name: 'test1' },
            continue: jest.fn()
        };

        const mockCursor2 = {
            value: { id: 2, name: 'test2' },
            continue: jest.fn()
        };

        const mockCursor3 = {
            value: { id: 3, name: 'test3' },
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown,
            addEventListener: (event: string, callback: EventListener) => void
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
        const promise = count(mockJson);

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
        await expect(promise).resolves.toBe(3); // All 3 records match
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledTimes(3);
        expect(mockCursor1.continue).toHaveBeenCalled();
        expect(mockCursor2.continue).toHaveBeenCalled();
        expect(mockCursor3.continue).toHaveBeenCalled();
    });

    it("should only count records that match the criteria", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(true)   // First record matches
            .mockReturnValueOnce(false)  // Second record doesn't match
            .mockReturnValueOnce(true);  // Third record matches

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn
        });

        let successCallback: EventListener | null = null;

        // Create mock cursors
        const mockCursor1 = {
            value: { id: 1, name: 'test1' },
            continue: jest.fn()
        };

        const mockCursor2 = {
            value: { id: 2, name: 'test2' },
            continue: jest.fn()
        };

        const mockCursor3 = {
            value: { id: 3, name: 'test3' },
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown,
            addEventListener: (event: string, callback: EventListener) => void
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
        const promise = count(mockJson);

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
        await expect(promise).resolves.toBe(2); // Only 2 records match
        expect(mockMatchesFn).toHaveBeenCalledTimes(3);
        expect(mockCursor1.continue).toHaveBeenCalled();
        expect(mockCursor2.continue).toHaveBeenCalled();
        expect(mockCursor3.continue).toHaveBeenCalled();
    });

    it("should return 0 when no records match the criteria", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(false);

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn
        });

        let successCallback: EventListener | null = null;

        const mockCursor = {
            value: { id: 1, name: 'test1' },
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
        const promise = count(mockJson);

        // Simulate cursor iterations
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        mockRequest.result = null; // End of records
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(0);
        expect(mockMatchesFn).toHaveBeenCalledTimes(1);
        expect(mockCursor.continue).toHaveBeenCalled();
    });

    it("should return 0 when there are no records", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn();

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
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
        const promise = count(mockJson);

        // Simulate success event with no records
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(0);
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
        const promise = count(mockJson);

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