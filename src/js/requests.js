let getHttpRequest = async (url, options = {}) => {
    try {
        // добавляю заголовок X-Requested-With к ajax вызовам чтобы их на сервере можно было идентифицировать
        // нужно для разделения выдачи ответа при неавторизованных запросах обычных и ajax
        options.headers = Object.assign(options.headers || {}, { 'X-Requested-With': 'XMLHttpRequest' });
        options.method = 'GET';
        let response = await fetch(url, options);

        if (response.ok) {
            return await response.text();
        } else {
            return 'Ошибка в приложении на сервере. Статус код ответа сервера: ' + response.status;
        }
    } catch (ex) {
        console.error(ex);
        return 'Ошибка при выполнении запроса на сервер. Подробные сведения отображены в консоли.';
    }
}

let getJsonRequest = (url) => {

}

export { getHttpRequest, getJsonRequest }