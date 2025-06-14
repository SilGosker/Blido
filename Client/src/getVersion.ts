import {connectToDatabase} from "./ConnectToDatabase";

export async function getVersion(database: string): Promise<number> {
    const connection = await connectToDatabase(database);
    return connection.version;
}
