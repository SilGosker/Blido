import { firstOrDefault } from "./firstOrDefault";
var BlazorOrmContext = /** @class */ (function () {
    function BlazorOrmContext() {
        this.firstOrDefault = firstOrDefault;
    }
    return BlazorOrmContext;
}());
window.blazorIndexedOrm = new BlazorOrmContext();
