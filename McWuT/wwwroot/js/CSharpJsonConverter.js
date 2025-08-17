(function(){
    const root = document.getElementById('csjson-root');
    if(!root) return;

    const convertBtn = document.getElementById('convertBtn');
    const btnText = convertBtn.querySelector('.btn-text');
    const btnSpinner = convertBtn.querySelector('.spinner-border');
    const detectedBadge = document.getElementById('detectedBadge');
    const discoverBtn = document.getElementById('discoverBtn');
    const typePicker = document.getElementById('typePicker');

    const inputArea = document.getElementById('inputArea');
    const outputTitle = document.getElementById('outputTitle');
    const outputCode = document.getElementById('outputCode');
    const inputValidation = document.getElementById('inputValidation');

    const optJsonProp = document.getElementById('optJsonProp');
    const optRootName = document.getElementById('optRootName');
    const optNamespace = document.getElementById('optNamespace');
    const optFileScoped = document.getElementById('optFileScoped');
    const optNullable = document.getElementById('optNullable');

    const minifyBtn = document.getElementById('minifyBtn');

    const alertContainer = document.getElementById('alertContainer');
    const alertMessage = document.getElementById('alertMessage');

    const copyToastEl = document.getElementById('copyToast');
    let copyToast;
    if (window.bootstrap && copyToastEl) {
        copyToast = new bootstrap.Toast(copyToastEl, { delay: 1200 });
    }

    function getAntiForgery(){
        const form = document.getElementById('__af');
        if(!form) return null;
        const input = form.querySelector('input[name="__RequestVerificationToken"]');
        return input?.value || null;
    }

    function detectMode(text){
        const t = (text || '').trim();
        if (!t) return 'none';
        if (t[0] === '{' || t[0] === '[') return 'json-to-csharp';
        if (/\b(class|struct|record)\b/.test(t) || /\bnamespace\b/.test(t) || /public\s+/.test(t)) return 'csharp-to-json';
        if (/\busing\b/.test(t)) return 'csharp-to-json';
        if (/\binterface\b/.test(t)) return 'csharp-to-json';
        if (/\bvoid\b/.test(t)) return 'csharp-to-json';
        if (/\bstatic\b/.test(t)) return 'csharp-to-json';
        if (/\bprivate\b/.test(t)) return 'csharp-to-json';
        if (/\bprotected\b/.test(t)) return 'csharp-to-json';
        if (/\binternal\b/.test(t)) return 'csharp-to-json';
        if (/\breadonly\b/.test(t)) return 'csharp-to-json';
        if (/\bnew\b/.test(t)) return 'csharp-to-json';
        if (/\bget;\b/.test(t)) return 'csharp-to-json';
        if (/\bset;\b/.test(t)) return 'csharp-to-json';
        if (/\binit;\b/.test(t)) return 'csharp-to-json';
        if (/\[\w+\]/.test(t)) return 'csharp-to-json';
        if (/\{\s*\}/.test(t)) return 'json-to-csharp';
        if (/\[\s*\]/.test(t)) return 'json-to-csharp';
        if (/\"\s*:\s*\"/.test(t)) return 'json-to-csharp';
        if (/\"\s*:\s*\d+/.test(t)) return 'json-to-csharp';
        if (/\"\s*:\s*true|false/.test(t)) return 'json-to-csharp';
        if (/\"\s*:\s*null/.test(t)) return 'json-to-csharp';
        return 'json-to-csharp';
    }

    function setDetected(mode){
        if(mode === 'none'){
            detectedBadge.classList.add('d-none');
            convertBtn.disabled = true;
            return;
        }
        convertBtn.disabled = false;
        detectedBadge.classList.remove('d-none');
        if(mode === 'json-to-csharp'){
            detectedBadge.textContent = 'Detected: JSON';
            outputCode.className = 'language-csharp';
            outputTitle.innerHTML = '<i class="bi bi-code-slash me-2"></i>C# Output';
        } else {
            detectedBadge.textContent = 'Detected: C#';
            outputCode.className = 'language-json';
            outputTitle.innerHTML = '<i class="bi bi-filetype-json me-2"></i>JSON Output';
        }
    }

    function showAlert(msg){
        alertMessage.textContent = msg || 'Invalid input';
        alertContainer.classList.remove('d-none');
        alertContainer.querySelector('.alert').classList.add('show');
    }

    function hideAlert(){
        alertContainer.classList.add('d-none');
    }

    window.CSJC = { hideAlert };

    async function copyToClipboard(text){
        try{ await navigator.clipboard.writeText(text); }
        catch{
            const ta = document.createElement('textarea'); ta.value = text; document.body.appendChild(ta); ta.select();
            try{ document.execCommand('copy'); } finally { document.body.removeChild(ta); }
        }
        if(copyToast) copyToast.show();
    }

    function download(text, ext){
        const ts = new Date().toISOString().replace(/[:.]/g,'-');
        const name = `output-${ts}.${ext}`;
        const type = ext === 'json' ? 'application/json' : 'text/plain';
        const blob = new Blob([text], { type: `${type};charset=utf-8` });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a'); a.href = url; a.download = name; document.body.appendChild(a); a.click(); document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }

    function setOutput(text){
        outputCode.textContent = text || '';
        if (window.Prism) Prism.highlightElement(outputCode);
    }

    function tryFormatOutput(minify){
        const mode = detectMode(inputArea.value);
        const text = outputCode.textContent || '';
        try{
            if(mode === 'csharp-to-json'){
                // JSON
                const obj = JSON.parse(text);
                setOutput(minify ? JSON.stringify(obj) : JSON.stringify(obj, null, 2));
            } else {
                // C# – naive formatter: trim and collapse extra blank lines for minify; just trim for beautify
                const lines = text.split(/\r?\n/);
                const compact = lines.filter((l,i,arr)=>!(l.trim()==='' && (i>0 && arr[i-1].trim()==='')));
                setOutput(minify ? compact.map(l=>l.trim()).join('') : compact.join('\n'));
            }
        }catch{ /* ignore formatting errors */ }
    }

    function toggleMaximize(which){
        const inputCol = document.getElementById('inputPane');
        const outputCol = document.getElementById('outputPane');
        if(which === 'input'){
            inputCol.classList.toggle('col-lg-12');
            inputCol.classList.toggle('col-lg-6');
            outputCol.classList.toggle('d-none');
        } else {
            outputCol.classList.toggle('col-lg-12');
            outputCol.classList.toggle('col-lg-6');
            inputCol.classList.toggle('d-none');
        }
    }

    document.getElementById('maximizeInputBtn').addEventListener('click', () => toggleMaximize('input'));
    document.getElementById('maximizeOutputBtn').addEventListener('click', () => toggleMaximize('output'));

    document.getElementById('copyInputBtn').addEventListener('click', () => copyToClipboard(inputArea.value));
    document.getElementById('copyOutputBtn').addEventListener('click', () => copyToClipboard(outputCode.textContent || ''));
    document.getElementById('downloadInputBtn').addEventListener('click', () => {
        const mode = detectMode(inputArea.value);
        const ext = mode === 'json-to-csharp' ? 'json' : 'cs';
        download(inputArea.value || '', ext);
    });
    document.getElementById('downloadOutputBtn').addEventListener('click', () => {
        const mode = detectMode(inputArea.value);
        const ext = mode === 'json-to-csharp' ? 'cs' : 'json';
        download(outputCode.textContent || '', ext);
    });

    async function postToHandler(handler, formData){
        const token = getAntiForgery();
        if(token) formData.set('__RequestVerificationToken', token);
        const res = await fetch(window.location.pathname + `?handler=${handler}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'X-Requested-With': 'XMLHttpRequest' },
            body: new URLSearchParams(formData).toString(),
            credentials: 'same-origin'
        });
        const html = await res.text();
        const parser = new DOMParser();
        const doc = parser.parseFromString(html, 'text/html');
        return doc.getElementById('csjson-data');
    }

    async function convert(){
        hideAlert();
        inputValidation.classList.add('d-none');
        inputValidation.textContent = '';
        const mode = detectMode(inputArea.value);
        if(mode === 'none') return;
        convertBtn.disabled = true; btnText.classList.add('d-none'); btnSpinner.classList.remove('d-none');

        const form = new FormData();
        form.set('Input', inputArea.value);
        form.set('Mode', mode);
        form.set('IncludeJsonPropertyName', optJsonProp.checked ? 'true' : 'false');
        form.set('RootClassName', optRootName.value || 'Root');
        form.set('Namespace', optNamespace.value || '');
        form.set('UseFileScopedNamespace', optFileScoped.checked ? 'true' : 'false');
        form.set('UseNullableContext', optNullable.checked ? 'true' : 'false');
        const targetTypeVal = typePicker.value || '';
        if(targetTypeVal) form.set('TargetType', targetTypeVal);

        try {
            const dataEl = await postToHandler('Convert', form);
            const error = dataEl?.dataset.error || '';
            const output = dataEl?.dataset.output || '';
            if (error) {
                showAlert(error);
                inputValidation.textContent = error;
                inputValidation.classList.remove('d-none');
                setOutput('');
            } else {
                setOutput(output);
            }
        } catch (e) {
            showAlert('Conversion failed');
            console.error(e);
        } finally {
            btnSpinner.classList.add('d-none'); btnText.classList.remove('d-none'); convertBtn.disabled = false;
        }
    }

    async function discover(){
        const mode = detectMode(inputArea.value);
        if(mode !== 'csharp-to-json') return;
        const form = new FormData();
        form.set('Input', inputArea.value);
        try {
            const dataEl = await postToHandler('Discover', form);
            const typesRaw = dataEl?.dataset.types || '';
            const types = typesRaw ? typesRaw.split('|').filter(Boolean) : [];
            typePicker.innerHTML = '';
            if(types.length > 0){
                for(const t of types){
                    const opt = document.createElement('option');
                    opt.value = t; opt.textContent = t; typePicker.appendChild(opt);
                }
                typePicker.classList.remove('d-none');
            } else {
                typePicker.classList.add('d-none');
            }
        }catch(e){ console.error(e); }
    }

    convertBtn.addEventListener('click', convert);
    discoverBtn.addEventListener('click', (e)=>{ e.preventDefault(); discover(); });

    function onInput(){
        const mode = detectMode(inputArea.value);
        setDetected(mode);
        if(mode !== 'csharp-to-json'){
            typePicker.classList.add('d-none');
        }
    }

    inputArea.addEventListener('input', onInput);
    onInput();

    minifyBtn.addEventListener('click', ()=> tryFormatOutput(true));
})();
