window.onload = highLightActivePlayer()

var roll = 0;

function rollDice() {
    if (window.roll == 0) {
        window.roll = Math.floor(Math.random() * 6) + 1;
        let rollButton = document.getElementById("roll-button");
        rollButton.innerHTML = window.roll;
        rollButton.removeEventListener("click", rollDice);
        rollButton.style.fontSize = "3vmin";
        rollButton.style.fontFamily = "sans-serif";
        rollButton.style.color = "white";
        rollButton.style.fontWeight = 700;

        getMovablePieces();
    }
}

async function getMovablePieces() {
    pieces = await fetchMovablePieces();
    pieces.forEach(
        
    );
}

async function fetchMovablePieces() {
    let result = await fetch('https://localhost:5001/api/game/boards/' + window.boardId+ '/players/' + window.activePlayerId + '/pieces/movable/' + window.roll, {
        mode:'no-cors',
        headers: {
            'apikey': 'Mountain24'
        }
    })
        .then(response => response.json())
        .then(data => {
            return data
        })
        .catch(error => {
            console.error(error);
        });
        console.log(result);
        console.log(result.results);
    return result.results;
}

function highLightActivePlayer() {
    let elementId = "ludoBase" + (window.activePlayerIndex + 1);
    document.getElementById(elementId).style.borderColor = "red";
}

document.getElementById("roll-button").addEventListener("click", rollDice);
