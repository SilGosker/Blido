// @ts-ignore
import {processArguments} from "../ProcessArguments";
import {startQuery} from "../StartQuery";

export function toArray(json: string) : Promise<unknown[]> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);

        const result: unknown[] = [];
        const request = await startQuery(args.databaseName, args.currentVersion, args.objectStoreName);

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(result);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                result.push(object);
                return;
            }

            cursor.continue();
        });
    });
}