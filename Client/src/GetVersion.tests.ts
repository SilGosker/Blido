import {getVersion} from "./GetVersion";
import { connectToDatabase } from "./ConnectToDatabase";

jest.mock("./ConnectToDatabase", () => ({
    connectToDatabase: jest.fn().mockResolvedValue({ version: 42 })
}));

describe("getVersion(database)", () => {

    it("should resolve with the version of the database", async () => {
        // Arrange
        const mockDatabase = "testDB";

        // Act
        const promise = getVersion(mockDatabase);

        // Assert
        await expect(promise).resolves.toBe(42);
        expect(connectToDatabase).toHaveBeenCalledWith(mockDatabase);
    });
});