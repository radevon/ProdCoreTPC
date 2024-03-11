
import '../css/base.css'
import '../css/materialize-icon-font.css'

import M from 'materialize-css'

import { openModalGetRequest, openModalPostRequest } from './modal.js'

import { submitFormModal } from './common.js'

document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.dropdown-trigger');
    var instances = M.Dropdown.init(elems, {});
});



window.openModalGetRequest = openModalGetRequest;
window.submitFormModal = submitFormModal;





