import { any } from "./Any";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartCursorModule from "../StartCursor";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartCursor", () => ({
    startCursor: jest.fn()
}));

describe("any(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it("should resolve with true when any record matches criteria", async () => {
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

        const mockCursor = {
            value: { id: 1, name: 'test1' },
            continue: jest.fn()
        };

        const mockRequest = {
            result: mockCursor,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = any(mockJson);

        // Simulate success event
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(true);
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledWith({ id: 1, name: 'test1' });
        expect(mockCursor.continue).not.toHaveBeenCalled(); // Should not continue since we found a match
    });

    it("should continue searching if record doesn't match criteria", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(false)  // First record doesn't match
            .mockReturnValueOnce(true);  // Second record matches

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

        const mockRequest = {
            result: mockCursor1,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = any(mockJson);

        // Simulate first success event (no match)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Update cursor to second item and trigger success again (match)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(true);
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledWith({ id: 1, name: 'test1' });
        expect(mockMatchesFn).toHaveBeenCalledWith({ id: 2, name: 'test2' });
        expect(mockCursor1.continue).toHaveBeenCalled();
        expect(mockCursor2.continue).not.toHaveBeenCalled(); // Should not continue since we found a match
    });

    it("should resolve with false when no records match criteria", async () => {
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

        // Create mock cursors
        const mockCursor1 = {
            value: { id: 1, name: 'test1' },
            continue: jest.fn()
        };

        const mockCursor2 = {
            value: { id: 2, name: 'test2' },
            continue: jest.fn()
        };

        const mockRequest: {
            result: unknown,
            addEventListener: (event: string, callback: EventListener) => void
        } = {
            result: mockCursor1,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = any(mockJson);

        // Simulate first success event (no match)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Update cursor to second item and trigger success again (no match)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Update cursor to null to indicate end of records and trigger success again
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(false);
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledWith({ id: 1, name: 'test1' });
        expect(mockMatchesFn).toHaveBeenCalledWith({ id: 2, name: 'test2' });
        expect(mockCursor1.continue).toHaveBeenCalled();
        expect(mockCursor2.continue).toHaveBeenCalled();
    });

    it("should resolve with false when there are no records", async () => {
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
                    successCallback = callback as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = any(mockJson);

        // Simulate success event with no records
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBe(false);
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
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
                    errorCallback = callback as EventListener;
                }
            })
        };

        (StartCursorModule.startCursor as jest.Mock).mockResolvedValue(mockRequest);

        // Act
        const promise = any(mockJson);

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