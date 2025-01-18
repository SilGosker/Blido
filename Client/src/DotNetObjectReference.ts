export interface DotNetObjectReference {
    invokeMethodAsync(methodName: string, ...args: any[]): Promise<any>;
    invokeMethodAsync<TResult>(methodName: string, ...args: any[]): Promise<TResult>;
    invokeMethod(methodName: string, ...args: any[]): any;
    invokeMethod<TResult>(methodName: string, ...args: any[]): TResult;
}