import M from 'materialize-css'
import { getHttpRequest, postFormDataRequest } from './requests.js'

/*
 *  <div id="modal1" class="modal">
    <div class="modal-content">
      <h4>Modal Header</h4>
      <div>Content</div>
    </div>
    <div class="modal-footer">
      <a href="#!" class="modal-close waves-effect waves-green btn-flat">Agree</a>
    </div>
  </div>


 * */

const MODAL_ID = 'MainModal';
/*
 * Формирование разметки модального окна, 
 * появление динамическое
 * */
let createMaterializeModal = (title, bodyContent = '', footerContent = '') => {

    let oldModal = document.querySelector('#' + MODAL_ID);
    if (oldModal !== null) {
        let modal_to_close = M.Modal.getInstance(oldModal);
        modal_to_close.close();
    }        

    let modal = document.createElement('div');
    modal.id = MODAL_ID;
    modal.className = 'modal';

    let m_content = document.createElement('div');
    m_content.className = 'modal-content center-align';


    let m_title = document.createElement('h4');
    m_title.innerText = title;

    let m_footer = document.createElement('div');
    m_footer.className = 'modal-footer';
    m_footer.innerHTML = footerContent;

    let closeBtn = document.createElement('button');
    closeBtn.classList.add('modal-close', 'btn-flat');
    closeBtn.style.position = 'absolute';
    closeBtn.style.top = '0';
    closeBtn.style.right = '0';
    closeBtn.innerText = 'X';
    

    let b_content = document.createElement('div');
    b_content.innerHTML = bodyContent;

    m_content.appendChild(m_title);
    m_content.appendChild(b_content);

    modal.appendChild(m_content);
    if (footerContent !== '')
        modal.appendChild(m_footer);

    modal.appendChild(closeBtn);
    document.body.appendChild(modal);

    executeScriptsInElement(b_content);

    return modal;
}

// Функция для выполнения скриптов в элементе
function executeScriptsInElement(element) {
    const scripts = element.querySelectorAll('script');
    scripts.forEach((script) => {
        const newScript = document.createElement('script');
        if (script.src) {
            newScript.src = script.src;
        } else {
            newScript.innerHTML = script.innerHTML;
        }
        element.appendChild(newScript);
    });
}


/*
 * Открытие модального окна с результатом GET http запроса
 * */
let openModalGetRequest = async (title, url, footer = '') => {
    let body = await getHttpRequest(url);
    M.Modal.init(
        createMaterializeModal(title, body, footer),
        {
            onCloseEnd: (el) => { el.remove(); }
        }
    ).open();
}
/* 
    Открытие модального окна с результатом POST http запроса
* */
let openModalPostRequest = async (title, url, formData, footer = '') => {
    let body = await postFormDataRequest(url, formData);
    M.Modal.init(
        createMaterializeModal(title, body, footer),
        {
            onCloseEnd: (el) => { el.remove(); }
        }
    ).open();
}

export { openModalGetRequest, openModalPostRequest }

