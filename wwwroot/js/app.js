// ══ StarFix — Client ══

const API = {
    state: '/api/state',
    action: '/api/action'
};

// ── DOM elements ──
const dom = {
    statsBar: document.getElementById('stats-bar'),
    playerName: document.getElementById('player-name'),
    playerScore: document.getElementById('player-score'),
    livesBar: document.getElementById('lives-bar'),
    livesText: document.getElementById('lives-text'),
    shipName: document.getElementById('ship-name'),
    healthBar: document.getElementById('health-bar'),
    healthText: document.getElementById('health-text'),
    title: document.getElementById('title'),
    messages: document.getElementById('messages'),
    inputArea: document.getElementById('input-area'),
    textInput: document.getElementById('text-input'),
    inputSubmit: document.getElementById('input-submit'),
    inventoryArea: document.getElementById('inventory-area'),
    options: document.getElementById('options'),
    mainCard: document.getElementById('main-card')
};

let currentInputAction = '';

// ── Fetch helpers ──
async function fetchState() {
    const res = await fetch(API.state);
    return res.json();
}

async function sendAction(action, value = '') {
    // Handle compound actions like "pick_level:2" or "answer:3"
    if (action.includes(':')) {
        const parts = action.split(':');
        action = parts[0];
        value = parts[1];
    }

    const res = await fetch(API.action, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ action, value })
    });
    return res.json();
}

// ── Render functions ──

function render(state) {
    // Update stats bar
    if (state.player && state.ship) {
        dom.statsBar.classList.remove('hidden');
        updateStats(state.player, state.ship);
    } else {
        dom.statsBar.classList.add('hidden');
    }

    // Title
    dom.title.textContent = state.title;
    dom.title.className = 'card-title';
    if (state.phase === 'GameWon' || state.phase === 'LevelComplete') {
        dom.title.classList.add('gold');
    } else if (state.phase === 'GameLost' || state.phase === 'LevelFailed') {
        dom.title.classList.add('danger');
    } else if (state.phase === 'ChallengeResult' && state.title.includes('✅')) {
        dom.title.classList.add('success');
    } else if (state.phase === 'ChallengeResult' && state.title.includes('❌')) {
        dom.title.classList.add('danger');
    }

    // Messages
    dom.messages.innerHTML = '';
    (state.messages || []).forEach(msg => {
        const p = document.createElement('p');
        if (msg === '' || msg === null) {
            p.className = 'empty';
        } else if (msg.startsWith('✅') || msg.startsWith('+') || msg.startsWith('🎉')) {
            p.className = 'success';
        } else if (msg.startsWith('❌') || msg.startsWith('⚠️') || msg.startsWith('💀')) {
            p.className = 'danger';
        } else if (msg.startsWith('📦') || msg.startsWith('  +')) {
            p.className = 'highlight';
        }
        p.textContent = msg;
        dom.messages.appendChild(p);
    });

    // Input
    if (state.showInput) {
        dom.inputArea.classList.remove('hidden');
        dom.textInput.placeholder = state.inputPlaceholder || '';
        dom.textInput.value = '';
        dom.textInput.focus();
        currentInputAction = state.inputAction || '';
    } else {
        dom.inputArea.classList.add('hidden');
    }

    // Inventory panel
    if (state.items && state.items.length > 0 && state.phase === 'ChallengeResult') {
        dom.inventoryArea.classList.remove('hidden');
        dom.inventoryArea.innerHTML = '';
        state.items.forEach(item => {
            const div = document.createElement('div');
            div.className = 'inv-item' + (item.isUsed ? ' used' : '');
            const icon = item.name.includes('Oxygen') ? '🫁' : '🔧';
            div.innerHTML = `
                <span class="inv-icon">${icon}</span>
                <div>
                    <div class="inv-name">${item.name}${item.isUsed ? ' [USED]' : ''}</div>
                    <div class="inv-desc">${item.description}</div>
                </div>
            `;
            dom.inventoryArea.appendChild(div);
        });
    } else {
        dom.inventoryArea.classList.add('hidden');
    }

    // Action buttons
    dom.options.innerHTML = '';
    (state.options || []).forEach(opt => {
        const btn = document.createElement('button');
        btn.className = `btn btn-${opt.style || 'secondary'}`;
        btn.textContent = opt.label;
        btn.addEventListener('click', async () => {
            btn.disabled = true;
            btn.style.opacity = '0.6';
            const newState = await sendAction(opt.id);
            // Re-animate the card
            dom.mainCard.style.animation = 'none';
            dom.mainCard.offsetHeight; // force reflow
            dom.mainCard.style.animation = 'fadeIn 0.35s ease';
            render(newState);
        });
        dom.options.appendChild(btn);
    });
}

function updateStats(player, ship) {
    dom.playerName.textContent = player.name;
    dom.playerScore.textContent = player.score;

    const livesPct = (player.lives / player.maxLives) * 100;
    dom.livesBar.style.width = livesPct + '%';
    dom.livesText.textContent = `${player.lives}/${player.maxLives}`;

    dom.shipName.textContent = ship.name;

    const healthPct = (ship.health / ship.maxHealth) * 100;
    dom.healthBar.style.width = healthPct + '%';
    dom.healthText.textContent = `${ship.health}/${ship.maxHealth}`;
}

// ── Input handling ──

dom.inputSubmit.addEventListener('click', submitInput);
dom.textInput.addEventListener('keydown', (e) => {
    if (e.key === 'Enter') submitInput();
});

async function submitInput() {
    const val = dom.textInput.value.trim();
    if (!val) return;
    dom.inputSubmit.disabled = true;
    const newState = await sendAction(currentInputAction, val);
    dom.inputSubmit.disabled = false;
    dom.mainCard.style.animation = 'none';
    dom.mainCard.offsetHeight;
    dom.mainCard.style.animation = 'fadeIn 0.35s ease';
    render(newState);
}

// ── Init ──
(async () => {
    const state = await fetchState();
    render(state);
})();
