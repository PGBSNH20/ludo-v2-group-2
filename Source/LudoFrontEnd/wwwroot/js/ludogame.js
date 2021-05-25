window.onload = highLightActivePlayer()

var roll = 0;
var movablePieces = [];

async function rollDice() {
    if (window.roll == 0) {
        window.roll = Math.floor(Math.random() * 6) + 1;
        let rollButton = document.getElementById("roll-button");
        rollButton.innerHTML = window.roll;
        rollButton.removeEventListener("click", rollDice);
        rollButton.style.fontSize = "3vmin";
        rollButton.style.fontFamily = "sans-serif";
        rollButton.style.color = "white";
        rollButton.style.fontWeight = 700;

        window.movablePieces = await getMovablePieces();
        highlightMovablePieces();
        highlightPossibleTargetSquares();
        addMoveClickListener();
        addMouseOverHighlight();
        await addLeaveBaseClick();
    }
}

function skipRound() {
    if (window.movablePieces.length == 0 && !canLeaveBase()) {
        apiSkipRound();
    }
}

async function addLeaveBaseClick() {
    removeLeaveBaseClicks();
    setTimeout(function() {
        if (canLeaveBase()) {
            let base = document.getElementById("ludoBase" + (activePlayerIndex + 1));
            base.style.cursor = "pointer";
            base.addEventListener("click", function() { leaveBase() }, false);
        }
    }, 5);
}

function removeLeaveBaseClicks() {
    let ludoBases = document.getElementsByClassName("ludoBase");
    for (const base of ludoBases) {
        base.style.cursor = "default";
        base.removeEventListener("click", function() { leaveBase() }, false);
    }
}

async function canLeaveBase() {
    if (window.roll != 6) {
        return false;
    }

    if (isOccupied(await apiGetStartPosition())) {
        return false;
    }

    if (getBasePieceCount() == 0) {
        return false;
    }

    return true;
}

function addMouseOverHighlight() {
    for (const piece of window.movablePieces) {
        let squarePiece = document.getElementById("sq_" + piece).getElementsByClassName("piece")[0];
        squarePiece.addEventListener("mouseover", highlightCurrentTargetOnly, false);
        squarePiece.addEventListener("mouseleave", highlightPossibleTargetSquares, false);
    }
}

function highlightCurrentTargetOnly(event) {
    removeAllSquareHighlights();
    let mouseoverSquare = event.relatedTarget.id;
    let originId = parseInt(mouseoverSquare.substring(3), 10);
    let targetId = originId + window.roll;
    highlightSquare("sq_" + targetId);
}

function highlightSquare(id) {
    let square = document.getElementById(id);
    square.classList.add("targetSquare");
}

function removeAllSquareHighlights() {
    let squares = document.getElementsByClassName("ludoSquare");
    for (const square of squares) {
        square.classList.remove("targetSquare");
    }
}

function highlightMovablePieces() {
    for (const piece of window.movablePieces) {
        let squarePiece = document.getElementById("sq_" + piece).getElementsByClassName("piece")[0];
        squarePiece.classList.add("movablePiece");
    }
}

function addMoveClickListener() {
    for (const piece of window.movablePieces) {
        let squarePiece = document.getElementById("sq_" + piece).getElementsByClassName("piece")[0];
        squarePiece.addEventListener("click", function() { apiMovePiece(piece) }, false);
    }
}

function highlightPossibleTargetSquares() {
    removeAllSquareHighlights();
    setTimeout(function() {
        for (const piece of window.movablePieces) {
            let targetSquare = piece + window.roll;
            let squarePiece = document.getElementById("sq_" + targetSquare);
            squarePiece.classList.add("targetSquare");
        }
    }, 5);
}

function highLightActivePlayer() {
    let elementId = "ludoBase" + (window.activePlayerIndex + 1);
    document.getElementById(elementId).style.borderColor = "red";
}

async function getMovablePieces() {
    pieces = await fetchMovablePieces();
    var movablePieces = [];
    pieces.forEach(piece => {
        movablePieces.push(piece["piecePosition"]);
    });
    return movablePieces;
}

async function getBasePieceCount() {
    const result = await apiFetch('boards/' + window.boardId + '/players/' + window.activePlayerId + '/pieces/base/', 'GET');
    console.log('getBasePieceCount: ' + result);
    return result.length;
}

async function isOccupied(position) {
    const result = await apiFetch('boards/' + window.boardId + '/squares/' + position + '/isOccupied/', 'GET');
    console.log('isOccupied: ' + result);
    return result;
}

async function leaveBase() {
    const result = await apiFetch('boards/' + window.boardId + '/players/' + window.activePlayerId + '/leaveBase', 'POST');
    console.log('leaveBase: ' + result);
    location.reload();
    return result;
}

async function apiMovePiece(position) {
    let pieceNumber = await getPieceNumberByPosition(position)
    const result = await apiFetch('boards/' + window.boardId + '/players/' + window.activePlayerId + '/pieces/' + pieceNumber + '/' + window.roll, 'POST');
    console.log('apiMovePiece: ' + result);
    location.reload();
    return result;
}

async function getPieceNumberByPosition(position) {
    const result = await apiFetch('boards/' + window.boardId + '/boardStates', 'GET');
    
    let pieceNumber = -1;
    result.forEach(state => {
        if(state["piecePosition"] == position) {
            pieceNumber = state["pieceNumber"];
            return false;
        }
    });

    console.log('getPieceNumberByPosition: ' + result);
    console.log('getPieceNumberByPosition(pieceNumber): ' + pieceNumber);
        
    return pieceNumber;
}

async function fetchMovablePieces() {
    const url = 'boards/' + window.boardId+ '/players/' + window.activePlayerId + '/pieces/movable/' + window.roll;
    const result = await apiFetch(url, 'GET');
    console.log('fetchMovablePieces(' + url + '): ' + result);
    return result;
}

async function apiSkipRound() {
    const result = await apiFetch('boards/' + window.boardId+ '/skipround', 'POST');
    console.log('apiSkipRound: ' + result);
    return result;
}

async function apiGetStartPosition() {
    const result = await apiFetch('boards/' + window.boardId+ '/players/' + window.activePlayerId + '/startingPosition', 'GET')
    console.log('apiGetStartPosition: ' + result);
    return result;
}

async function apiFetch(url, verb) {
    verb = verb.toUpperCase();
    url = 'https://localhost:5001/api/game/' + url;
    const result = await fetch(url,
    {
        method: verb,
        headers: {
            "apikey": "Mountain24"
        }
    })
    .then(response => response.json())
    .then(data => {
        return data
    })
    .catch(error => {
        console.error(error);
    });
    return result;
}

document.getElementById("roll-button").addEventListener("click", rollDice);
