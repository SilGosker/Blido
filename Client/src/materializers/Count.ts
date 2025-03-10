// @ts-ignore
import {processArguments} from "../ProcessArguments";
import {startCursor} from "../StartCursor";

export function count(json: string) : Promise<number> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);

        let count = 0;
        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(count);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                count++;
            }

            cursor.continue();
        });
    });
}