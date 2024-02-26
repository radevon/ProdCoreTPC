import M from 'materialize-css'

let createMaterializeModal = (title = 'Модальное окно', bodyContent = '', footerContent = '', id = 'MainModal') => {

    let old = document.querySelector('#' + id);
    if (old !== null)
        document.body.removeChild(old);

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

    let b_content = document.createElement('div');
    b_content.innerHTML = bodyContent;

    m_content.appendChild(m_title);
    m_content.appendChild(b_content);

    modal.appendChild(m_content);
    modal.appendChild(m_footer);

    document.body.appendChild(modal);
    return modal;
}

export { createMaterializeModal }

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