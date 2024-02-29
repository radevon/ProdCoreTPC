import M from 'materialize-css'
import { getHttpRequest } from './requests.js'

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

/*
 * Формирование разметки модального окна, 
 * появление динамическое
 * */
let createMaterializeModal = (title, bodyContent = '', footerContent = '', id = 'MainModal') => {

    let old = document.querySelector('#' + id);
    if (old !== null)
        old.remove();

    let modal = document.createElement('div');
    modal.id = id;
    modal.className = 'modal';

    let m_content = document.createElement('div');
    m_content.className = 'modal-content';


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
    return modal;
}


/*
 * Открытие модального окна с результатом http запроса
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

export { openModalGetRequest }

