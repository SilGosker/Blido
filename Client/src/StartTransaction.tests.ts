import * as connectToDatabaseModule from "./ConnectToDatabase";
import { startTransaction } from "./StartTransaction";

jest.mock("./ConnectToDatabase", () => ({
    connectToDatabase: jest.fn()
}));

describe('startTransaction(database, currentVersion, objectStore, readonly)', () => {
    const mockDatabaseName = 'testDB';
    const mockVersion = 1;
    const mockObjectStore = 'users';

    beforeEach(() => {
        jest.clearAllMocks();
    });

    it('should start a readonly transaction by default', async () => {
        // Arrange
        const mockTransaction = { objectStore: jest.fn().mockReturnValue(true) };
        const mockDb = {
            transaction: jest.fn().mockReturnValue(mockTransaction)
        };
        (connectToDatabaseModule.connectToDatabase as jest.Mock).mockResolvedValue(mockDb);

        // Act
        const result = await startTransaction(mockDatabaseName, mockVersion, mockObjectStore);

        // Assert
        expect(result).toBeTruthy();
        expect(connectToDatabaseModule.connectToDatabase).toHaveBeenCalledWith(mockDatabaseName, mockVersion);
        expect(mockDb.transaction).toHaveBeenCalledWith(mockObjectStore, 'readonly');
        expect(mockTransaction.objectStore).toHaveBeenCalledWith(mockObjectStore);
    });

    it('should start a readwrite transaction when readonly is false', async () => {
        // Arrange
        const mockTransaction = { objectStore: jest.fn().mockReturnValue(true) };
        const mockDb = {
            transaction: jest.fn().mockReturnValue(mockTransaction)
        };
        (connectToDatabaseModule.connectToDatabase as jest.Mock).mockResolvedValue(mockDb);

        // Act
        const result = await startTransaction(mockDatabaseName, mockVersion, mockObjectStore, false);

        // Assert
        expect(result).toBeTruthy();
        expect(connectToDatabaseModule.connectToDatabase).toHaveBeenCalledWith(mockDatabaseName, mockVersion);
        expect(mockDb.transaction).toHaveBeenCalledWith(mockObjectStore, 'readwrite');
        expect(mockTransaction.objectStore).toHaveBeenCalledWith(mockObjectStore);
    });
});