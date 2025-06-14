import {processQueryArguments} from "../ProcessQueryArguments";
import {startTransaction} from "../StartTransaction";

export function find(json: string) : Promise<number> {
    return new Promise(async (resolve, reject) => {
        const args = processQueryArguments(json);

        const request = (await startTransaction(args.databaseName, args.currentVersion, args.objectStoreName))
            .get(args.id as IDBValidKey);

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            resolve(request.result);
        });
    });
}