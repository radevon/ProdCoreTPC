/*
 * Метод для отправки http запроса GET с возможными параметрами в строке url и получением ответа в виде text/html
 * */
let getHttpRequest = async (url) => {
    // добавляю заголовок X-Requested-With к ajax вызовам чтобы их на сервере можно было идентифицировать
    // нужно для разделения выдачи ответа при неавторизованных запросах обычных и ajax
    let options = { method: 'GET', headers: { 'X-Requested-With': 'XMLHttpRequest' }};
    try {
        let response = await fetch(url, options);
        if (response.ok) 
            return await response.text();
        else 
            return 'Ошибка в приложении на сервере. Статус код ответа сервера: ' + response.status;
    } catch (ex) {
        console.error(ex);
        return 'Ошибка при выполнении http GET запроса на сервер. Подробные сведения отображены в консоли.';
    }
}



/*
 * Метод для отправки http запроса GET с возможными параметрами в строке url и получением ответа в виде json 
 * */
let getJsonRequest = async(url) => {
    let options = { method: 'GET', headers: { 'X-Requested-With': 'XMLHttpRequest' } };
    // структура ответа всегда такая
    let result = {
        isSuccess: true,
        data: null,
        message:''
    };
    try {
        let response = await fetch(url, options);

        if (response.ok) {
            result.data=await response.json();
        } else {
            result.isSuccess = false;
            result.message = 'Ошибка в приложении на сервере. Статус код ответа сервера: ' + response.status;
        }
    } catch (ex) {
        console.error(ex);
        result.isSuccess = false;
        result.message = 'Ошибка при выполнении json GET запроса на сервер.' + ex.message;
    }
}

/*
 * Метод для отправки http запроса POST с параметрами в теле запроса с типом сериализации application/json и получением ответа в виде text/html
 * */
let postJsonRequest = async (url,parameters) => {
    let options = {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(parameters)
    };
    try {
        let response = await fetch(url, options);
        if (response.ok)
            return await response.text();
        else
            return 'Ошибка в приложении на сервере. Статус код ответа сервера: ' + response.status;
    } catch (ex) {
        console.error(ex);
        return 'Ошибка при выполнении http POST запроса на сервер. Подробные сведения отображены в консоли.';
    }
}

let postFormDataRequest = async (url, formData) => {
    let options = {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: formData
    };
    try {
        let response = await fetch(url, options);
        if (response.ok)
            return await response.text();
        else
            return 'Ошибка в приложении на сервере. Статус код ответа сервера: ' + response.status;
    } catch (ex) {
        console.error(ex);
        return 'Ошибка при выполнении http POST запроса на сервер. Подробные сведения отображены в консоли.';
    }
}

export { getHttpRequest, getJsonRequest, postJsonRequest, postFormDataRequest }