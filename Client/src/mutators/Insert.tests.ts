import { insert } from "./Insert";
import * as ProcessMutateArgumentsModule from "../ProcessMutateArguments";
import * as StartTransactionModule from "../StartTransaction";

jest.mock("../ProcessMutateArguments", () => ({
    processMutateArguments: jest.fn().mockReturnValue({
        database: 'testDB',
        version: 1,
        objectStore: 'testObjectStore',
        entity: { id: 0, name: 'test' },
        primaryKeys: ["id"]
    })
}));

jest.mock("../StartTransaction", () => ({
    startTransaction: jest.fn()
}));

describe("insert(json)", () => {
    it("should resolve with the inserted entity and primary key", async () => {
        // Arrange
        const mockJson = "{}";
        let successCallback: EventListener | undefined;
        const mockRequest = {
            get result() { return 123; },
            set onsuccess(cb: EventListener) { successCallback = cb; }
        };
        const mockObjectStore = {
            add: jest.fn().mockReturnValue(mockRequest),
        };

        (StartTransactionModule.startTransaction as jest.Mock).mockReturnValue(mockObjectStore);

        // Act
        const promise = insert(mockJson);
        await Promise.resolve();
        (successCallback as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toEqual({ name: "test", id: 123 });
        expect(ProcessMutateArgumentsModule.processMutateArguments).toHaveBeenCalledWith(mockJson);
        expect(StartTransactionModule.startTransaction).toHaveBeenCalledWith("testDB", 1, "testObjectStore", false);
        expect(mockObjectStore.add).toHaveBeenCalledWith({name: "test", id: 0});
    });

    it("should reject with the error", async () => {
        // Arrange
        const mockJson = "{}";
        let errorCallback: EventListener | undefined;
        const mockRequest = {
            get error() { return "An error occurred"; },
            set onerror(cb: EventListener) { errorCallback = cb; }
        };
        const mockObjectStore = {
            add: jest.fn().mockReturnValue(mockRequest),
        };

        (StartTransactionModule.startTransaction as jest.Mock).mockReturnValue(mockObjectStore);

        // Act
        const promise = insert(mockJson);
        await Promise.resolve();
        (errorCallback as EventListener)(new Event("error"));

        // Assert
        await expect(promise).rejects.toBe("An error occurred");
        expect(ProcessMutateArgumentsModule.processMutateArguments).toHaveBeenCalledWith(mockJson);
        expect(StartTransactionModule.startTransaction).toHaveBeenCalledWith("testDB", 1, "testObjectStore", false);
        expect(mockObjectStore.add).toHaveBeenCalledWith({name: "test", id: 0});
    });
});