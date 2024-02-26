
import '../css/base.css'
import '../css/materialize-icon-font.css'

import M from 'materialize-css'

document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.dropdown-trigger');
    var instances = M.Dropdown.init(elems, {});
});

import { createMaterializeModal } from './modal.js'


document.querySelector('nav').onclick = function () {
    let instance = M.Modal.init(createMaterializeModal('Мой заголовок', 'Содержимое', 'footer'), {});
    instance.open();
}



