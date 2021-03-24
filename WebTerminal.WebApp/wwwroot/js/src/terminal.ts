function initializeTerminalClicks() {
    let terminal = <HTMLElement>document.getElementsByClassName("terminal")[0];
    let input = <HTMLElement>document.getElementsByClassName("input")[0];

    terminal.onclick = () => {
        let selection = window.getSelection();
        let selectionRange = selection?.getRangeAt(0);
        let selectedString = selectionRange?.toString();
        if (selectedString == null || selectedString == "") {
            input.focus();
        }
    };
}

function scrollToBottom() {
    window.scrollTo(0, document.body.scrollHeight);
}