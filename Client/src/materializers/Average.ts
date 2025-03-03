// @ts-ignore
import {processArguments} from "../ProcessArguments";
import {startQuery} from "../StartQuery";

export function average(json: string) : Promise<number> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);

        const request = await startQuery(args.databaseName, args.currentVersion, args.objectStoreName);
        let sum = 0;
        let count = 0;

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(sum / count);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                sum += args.selector(object) as number;
                count++;
                return;
            }

            cursor.continue();
        });
    });
}