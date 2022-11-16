export interface IServiceResult<TData> {
    status: number;
    data?: TData;
    errorMessage?: string;

}