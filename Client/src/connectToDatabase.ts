export function connectToDatabase(name: string, currentVersion : undefined | number  = undefined) : Promise<IDBDatabase> {
    return new Promise((resolve, reject) => {
        const request = window.indexedDB.open(name, currentVersion);
        request.addEventListener('success', () => {
            resolve(request.result);
        });
        request.addEventListener('error', reject);
    });
}