import { FirstOrDefaultInterface, firstOrDefault } from "./materializers/firstOrDefault";
import {GetVersionInterface, getVersion} from "./getVersion";

class BlazorOrmContext {
    public firstOrDefault: FirstOrDefaultInterface
    public getVersion : GetVersionInterface
    public constructor() {
        this.firstOrDefault = firstOrDefault;
        this.getVersion = getVersion;
    }
}

(window as any).blazorIndexedOrm = new BlazorOrmContext();