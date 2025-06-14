import { find } from "./Find";
import * as ProcessQueryArgumentsModule from "../ProcessQueryArguments";
import * as StartTransactionModule from "../StartTransaction";

jest.mock("../ProcessQueryArguments", () => ({
    processQueryArguments: jest.fn()
}));

jest.mock("../StartTransaction", () => ({
    startTransaction: jest.fn()
}));

describe("find(json)", () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it("should resolve with the found record", async () => {
        // Arrange
        const mockJson = "{}";
        const mockRecord = { id: 123, name: 'test record' };

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            id: 123
        });

        let successCallback: EventListener | null = null;

        const mockRequest = {
            result: mockRecord,
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as unknown as EventListener;
                }
            })
        };

        const mockObjectStore = {
            get: jest.fn().mockReturnValue(mockRequest)
        };

        (StartTransactionModule.startTransaction as jest.Mock).mockResolvedValue(mockObjectStore);

        // Act
        const promise = find(mockJson);

        // Simulate success event
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toEqual(mockRecord);
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartTransactionModule.startTransaction).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockObjectStore.get).toHaveBeenCalledWith(123);
    });

    it("should resolve with undefined when record is not found", async () => {
        // Arrange
        const mockJson = "{}";

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            id: 123
        });

        let successCallback: EventListener | null = null;

        const mockRequest = {
            result: undefined, // No result means record not found
            addEventListener: jest.fn((event, callback) => {
                if (event === 'success') {
                    successCallback = callback as unknown as EventListener;
                }
            })
        };

        const mockObjectStore = {
            get: jest.fn().mockReturnValue(mockRequest)
        };

        (StartTransactionModule.startTransaction as jest.Mock).mockResolvedValue(mockObjectStore);

        // Act
        const promise = find(mockJson);

        // Simulate success event
        await Promise.resolve();
        (successCallback as unknown as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toBeUndefined();
        expect(ProcessQueryArgumentsModule.processQueryArguments).toHaveBeenCalledWith(mockJson);
        expect(StartTransactionModule.startTransaction).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockObjectStore.get).toHaveBeenCalledWith(123);
    });

    it("should reject when an error occurs", async () => {
        // Arrange
        const mockJson = "{}";
        const mockError = new Error("Database error");

        (ProcessQueryArgumentsModule.processQueryArguments as jest.Mock).mockReturnValue({
            databaseName: 'testDB',
            currentVersion: 1,
            objectStoreName: 'testObjectStore',
            id: 123
        });

        let errorCallback: EventListener | null = null;

        const mockRequest = {
            addEventListener: jest.fn((event, callback) => {
                if (event === 'error') {
                    errorCallback = callback as unknown as EventListener;
                }
            })
        };

        const mockObjectStore = {
            get: jest.fn().mockReturnValue(mockRequest)
        };

        (StartTransactionModule.startTransaction as jest.Mock).mockResolvedValue(mockObjectStore);

        // Act
        const promise = find(mockJson);

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
        expect(StartTransactionModule.startTransaction).toHaveBeenCalledWith('testDB', 1, 'testObjectStore');
        expect(mockObjectStore.get).toHaveBeenCalledWith(123);
    });
});