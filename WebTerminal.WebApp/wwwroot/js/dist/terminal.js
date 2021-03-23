function initializeTerminalClicks() {
    var terminal = document.getElementsByClassName("terminal")[0];
    var input = document.getElementsByClassName("input")[0];
    terminal.onclick = function () {
        var selection = window.getSelection();
        var selectionRange = selection === null || selection === void 0 ? void 0 : selection.getRangeAt(0);
        var selectedString = selectionRange === null || selectionRange === void 0 ? void 0 : selectionRange.toString();
        if (selectedString == null || selectedString == "") {
            input.focus();
        }
    };
}
//# sourceMappingURL=terminal.js.map