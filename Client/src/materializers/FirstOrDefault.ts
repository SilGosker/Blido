import {processQueryArguments} from "../ProcessQueryArguments";
import {startCursor} from "../StartCursor";

export function firstOrDefault(json: string) : Promise<unknown> {
    return new Promise(async (resolve, reject) => {
        const args = processQueryArguments(json);

        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);
        request.addEventListener('error', reject);

        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(null);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                resolve(object);
                return;
            }

            cursor.continue();
        });
    });
}