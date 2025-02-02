import {connectToDatabase} from "./connectToDatabase";

export interface GetVersionInterface {
    (database: string) : Promise<number>;
}

export function getVersion(database : string): Promise<number> {
    return new Promise(async (resolve) => {
        const connection = await connectToDatabase(database);
        resolve(connection.version)
    });

}