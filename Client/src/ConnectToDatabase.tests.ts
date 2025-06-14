import {connectToDatabase} from "./ConnectToDatabase";

describe("connectToDatabase(string, currentVersion)", () => {
    const mockOpenIndexedDbRequest = {
        get result() {
            return "opened database request";
        },
        get error() {
            return "An error occurred";
        }
    };
    const mockOpenIndexedDb = jest.fn().mockReturnValue(mockOpenIndexedDbRequest);

    beforeAll(() => {
        Object.defineProperty(window, "indexedDB", {
            value: {
                open: mockOpenIndexedDb,
            }
        });
    });

    it("Should return a promise with result", async () => {
        // Arrange
        let successCallback: EventListener | undefined;
        Object.defineProperty(mockOpenIndexedDbRequest, "onsuccess", {
            set(cb: EventListener) {
                successCallback = cb;
            },
            configurable: true,
        });

        // Act
        const promise = connectToDatabase("test");
        (successCallback as EventListener)(new Event("success"));
        // Assert
        await expect(promise).resolves.toEqual("opened database request");
        expect(mockOpenIndexedDb).toHaveBeenCalledWith("test", undefined);
    });

    it("Should return a promise with result when version is specified", async () => {
        // Arrange
        let successCallback: EventListener | undefined;
        Object.defineProperty(mockOpenIndexedDbRequest, "onsuccess", {
            set(cb: EventListener) {
                successCallback = cb;
            },
            configurable: true,
        });

        // Act
        const promise = connectToDatabase("test", 1);
        (successCallback as EventListener)(new Event("success"));

        // Assert
        await expect(promise).resolves.toEqual("opened database request");
        expect(mockOpenIndexedDb).toHaveBeenCalledWith("test", 1);
    });

    it("Should return a promise with error", async () => {
        // Arrange
        let errorCallback: EventListener | undefined;
        Object.defineProperty(mockOpenIndexedDbRequest, "onerror", {
            set(cb: EventListener) {
                errorCallback = cb;
            },
            configurable: true,
        });

        // Act
        const promise = connectToDatabase("test");
        (errorCallback as EventListener)(new Event("error"));

        // Assert
        await expect(promise).rejects.toEqual("An error occurred");
        expect(mockOpenIndexedDb).toHaveBeenCalledWith("test", undefined);
    });
});
