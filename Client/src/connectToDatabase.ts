export function connectToDatabase(name: string, currentVersion : undefined | number  = undefined) : Promise<IDBDatabase> {
    return new Promise((resolve, reject) => {
        const request = window.indexedDB.open(name, currentVersion);

        request.onsuccess = () => resolve(request.result);
        request.onerror = () => reject(request.error);
    });
}