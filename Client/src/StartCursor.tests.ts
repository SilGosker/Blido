import {connectToDatabase} from "./ConnectToDatabase";
import {startCursor} from "./StartCursor";

jest.mock('./ConnectToDatabase', () => ({
    connectToDatabase: jest.fn()
}));

describe('startCursor(database, currentVersion, objectStore)', () => {
    const mockDatabaseName = 'testDB';
    const mockVersion = 1;
    const mockObjectStore = 'users';

    const mockCursor = {};

    const mockObjectStoreInstance = {
        openCursor: jest.fn().mockReturnValue(mockCursor)
    };

    const mockTransaction = {
        objectStore: jest.fn().mockReturnValue(mockObjectStoreInstance)
    };

    const mockDatabase = {
        transaction: jest.fn().mockReturnValue(mockTransaction)
    };

    beforeEach(() => {
        jest.clearAllMocks();

        (connectToDatabase as jest.Mock).mockResolvedValue(mockDatabase);
    });

    it('should open a cursor for the specified object store', async () => {
        // Act
        const result = await startCursor(mockDatabaseName, mockVersion, mockObjectStore);

        // Assert
        expect(connectToDatabase).toHaveBeenCalledWith(mockDatabaseName, mockVersion);
        expect(mockDatabase.transaction).toHaveBeenCalledWith(mockObjectStore, 'readonly');
        expect(mockTransaction.objectStore).toHaveBeenCalledWith(mockObjectStore);
        expect(mockObjectStoreInstance.openCursor).toHaveBeenCalled();
        expect(result).toBe(mockCursor);
    });
});