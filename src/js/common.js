import { openModalPostRequest } from './modal.js'

// отправка формы из модали
let submitFormModal = async (event, title = '', footer = '') => {
    event.preventDefault();
    let form = event.target;
    await openModalPostRequest(title, form.action, new FormData(form), footer);
}

export {
    submitFormModal
}