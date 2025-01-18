import { firstOrDefaultInterface, firstOrDefault } from "./firstOrDefault";

class BlazorOrmContext {
    private firstOrDefault: firstOrDefaultInterface

    public constructor() {
        this.firstOrDefault = firstOrDefault;
    }
}

(window as any).blazorIndexedOrm = new BlazorOrmContext();