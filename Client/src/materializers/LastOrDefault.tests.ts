import { lastOrDefault } from "./LastOrDefault";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartCursorModule from "../StartCursor";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartCursor", () => ({
    startCursor: jest.fn()
}));

describe("lastOrDefault(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it("should resolve with the last matching record when only one matches", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn().mockReturnValue(true);
        const mockRecord = { id: 1, name: 'test1' };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn
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
        const promise = lastOrDefault(mockJson);

        // Simulate first success event (match found)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate end of cursor (no more records)
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toEqual(mockRecord);
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartCursorModule.startCursor).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord);
        expect(mockCursor.continue).toHaveBeenCalled(); // Should continue to find potential later matches
    });

    it("should resolve with the last matching record when multiple match", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(true)   // First record matches
            .mockReturnValueOnce(false)  // Second record doesn't match
            .mockReturnValueOnce(true);  // Third record matches (this should be the last)

        const mockRecord1 = { id: 1, name: 'test1' };
        const mockRecord2 = { id: 2, name: 'test2' };
        const mockRecord3 = { id: 3, name: 'test3' };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn
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
        const promise = lastOrDefault(mockJson);

        // Simulate first success event (first match)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate second success event (no match)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate third success event (second match - this should be the last)
        mockRequest.result = mockCursor3;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate end of cursor (no more records)
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toEqual(mockRecord3);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord3);
        expect(mockCursor1.continue).toHaveBeenCalled();
        expect(mockCursor2.continue).toHaveBeenCalled();
        expect(mockCursor3.continue).toHaveBeenCalled();
    });

    it("should continue through all records even if none match initially", async () => {
        // Arrange
        const mockJson = "{}";
        const mockMatchesFn = jest.fn()
            .mockReturnValueOnce(false)  // First record doesn't match
            .mockReturnValueOnce(false)  // Second record doesn't match
            .mockReturnValueOnce(true);  // Third record matches

        const mockRecord1 = { id: 1, name: 'test1' };
        const mockRecord2 = { id: 2, name: 'test2' };
        const mockRecord3 = { id: 3, name: 'test3' };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            matches: mockMatchesFn
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
        const promise = lastOrDefault(mockJson);

        // Simulate first success event (no match)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate second success event (no match)
        mockRequest.result = mockCursor2;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate third success event (match)
        mockRequest.result = mockCursor3;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Simulate end of cursor
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toEqual(mockRecord3);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord1);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord2);
        expect(mockMatchesFn).toHaveBeenCalledWith(mockRecord3);
    });

    it("should resolve with null when no records match criteria", async () => {
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
        const promise = lastOrDefault(mockJson);

        // Simulate first success event (no match)
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Update cursor to null to indicate end of records and trigger success again
        mockRequest.result = null;
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBeNull();
        expect(mockCursor.continue).toHaveBeenCalled();
    });

    it("should resolve with null when there are no records", async () => {
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
        const promise = lastOrDefault(mockJson);

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
        const promise = lastOrDefault(mockJson);

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