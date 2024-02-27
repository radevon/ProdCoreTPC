
import '../css/base.css'
import '../css/materialize-icon-font.css'

import M from 'materialize-css'

document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.dropdown-trigger');
    var instances = M.Dropdown.init(elems, {});
});

import { openModalRequest } from './modal.js'

window.openModalRequest = openModalRequest;





