namespace SampleWebApplication.Models{
    export class SampleModel{
        Id : number;
        Name : string;
        Note : string;
    }
}


namespace SampleWebApplication.Controllers{
    
    export class HomeController{
        static Remove(id: number, then: (model: SampleWebApplication.Models.SampleModel[])=>void, fail: (message: string)=>void,token:string = null): void{
            let __arg = {
                id : id,
            };
            this.__loadModel('./Home/Remove', __arg, then, fail, token);
        }
        
        static Add(name: string, note: string, then: (model: SampleWebApplication.Models.SampleModel[])=>void, fail: (message: string)=>void,token:string = null): void{
            let __arg = {
                name : name,
                note : note,
            };
            this.__loadModel('./Home/Add', __arg, then, fail, token);
        }
        
        static Edit(id: number, name: string, note: string, then: (model: SampleWebApplication.Models.SampleModel[])=>void, fail: (message: string)=>void,token:string = null): void{
            let __arg = {
                id : id,
                name : name,
                note : note,
            };
            this.__loadModel('./Home/Edit', __arg, then, fail, token);
        }
        
        static __convert(text: string): string {
            let dict = [
            ];
            for (let d of dict) {
                text = text.replace(d[0], d[1]);
            }
            return text;
        }
        
        static __loadModel<TArg, TModel>(
        url: string,
        arg: TArg,
        then: (model: TModel) => void,
        fail: (message: string) => void,
        token:string = null): void{
            let xhr = new XMLHttpRequest();
            xhr.onreadystatechange = () => {
                if (xhr.readyState == 4) {
                    let message = 'unkwon error.';
                    if (xhr.status == 200) {
                        try {
                            let jsonText = this.__convert(xhr.responseText);
                            then(JSON.parse(jsonText));
                            return;
                        } catch (ex) {
                            if (ex instanceof Error) {
                                message = ex.message;
                            }
                        }
                    } else {
                        fail(message);
                    }
                }
            };
            xhr.open('POST', url, true);
            xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            if (token)
                xhr.setRequestHeader('__RequestVerificationToken', token);
            xhr.send(JSON.stringify(arg));
        }
    }
}

