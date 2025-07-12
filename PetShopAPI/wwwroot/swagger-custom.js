// Custom JavaScript for PetShop Swagger UI
(function() {
    'use strict';

    // Wait for DOM to be ready
    document.addEventListener('DOMContentLoaded', function() {
        
        // Add custom header with PetShop branding
        function addCustomHeader() {
            const infoSection = document.querySelector('.swagger-ui .info');
            if (infoSection && !document.querySelector('.petshop-custom-header')) {
                const header = document.createElement('div');
                header.className = 'petshop-custom-header';
                header.innerHTML = `
                    <div style="text-align: center; margin-bottom: 20px; padding: 20px; background: linear-gradient(135deg, #2E8B57, #4ECDC4); border-radius: 12px; color: white;">
                        <h1 style="margin: 0; font-size: 2.5rem; text-shadow: 2px 2px 4px rgba(0,0,0,0.3);">
                            🐾 PetShop Hybrid API
                        </h1>
                        <p style="margin: 10px 0 0 0; font-size: 1.2rem; opacity: 0.9;">
                            Powered by .NET 9 | SQL Server + MongoDB Architecture
                        </p>
                        <div style="margin-top: 15px; display: flex; justify-content: center; gap: 15px; flex-wrap: wrap;">
                            <span style="background: rgba(255,255,255,0.2); padding: 5px 12px; border-radius: 20px; font-size: 0.9rem;">
                                📊 SQL Server: Business Data
                            </span>
                            <span style="background: rgba(255,255,255,0.2); padding: 5px 12px; border-radius: 20px; font-size: 0.9rem;">
                                🍃 MongoDB: Authentication
                            </span>
                            <span style="background: rgba(255,255,255,0.2); padding: 5px 12px; border-radius: 20px; font-size: 0.9rem;">
                                🔒 Token-Based Security
                            </span>
                        </div>
                    </div>
                `;
                infoSection.parentNode.insertBefore(header, infoSection);
            }
        }

        // Add helpful tips for authentication
        function addAuthTips() {
            const authButton = document.querySelector('.auth-wrapper .authorize');
            if (authButton && !authButton.dataset.tipsAdded) {
                authButton.dataset.tipsAdded = 'true';
                authButton.addEventListener('click', function() {
                    setTimeout(() => {
                        const modal = document.querySelector('.auth-container');
                        if (modal && !modal.querySelector('.auth-tips')) {
                            const tips = document.createElement('div');
                            tips.className = 'auth-tips';
                            tips.innerHTML = `
                                <div style="background: #e3f2fd; border: 1px solid #2196f3; border-radius: 8px; padding: 15px; margin: 15px 0;">
                                    <h4 style="margin: 0 0 10px 0; color: #1976d2;">💡 Dicas de Autenticação:</h4>
                                    <ul style="margin: 0; padding-left: 20px; color: #333;">
                                        <li><strong>Cliente:</strong> Use <code>/api/auth/cliente/login</code></li>
                                        <li><strong>Admin:</strong> Use <code>/api/auth/admin/login</code></li>
                                        <li><strong>Formato:</strong> Copie apenas o token (sem "Bearer")</li>
                                        <li><strong>Teste:</strong> admin1@petshop.com / admin123</li>
                                    </ul>
                                </div>
                            `;
                            modal.appendChild(tips);
                        }
                    }, 300);
                });
            }
        }

        // Enhance operation summaries with icons
        function enhanceOperations() {
            const operations = document.querySelectorAll('.opblock-summary');
            operations.forEach(op => {
                if (!op.dataset.enhanced) {
                    op.dataset.enhanced = 'true';
                    const method = op.querySelector('.opblock-summary-method');
                    const path = op.querySelector('.opblock-summary-path');
                    
                    if (method && path) {
                        const pathText = path.textContent;
                        let icon = '';
                        
                        // Add icons based on endpoint
                        if (pathText.includes('/auth/')) icon = '🔐';
                        else if (pathText.includes('/produtos')) icon = '🛍️';
                        else if (pathText.includes('/categorias')) icon = '🏷️';
                        else if (pathText.includes('/carrinho')) icon = '🛒';
                        else if (pathText.includes('/pedidos')) icon = '📦';
                        else if (pathText.includes('/clientes')) icon = '👥';
                        
                        if (icon) {
                            const summary = op.querySelector('.opblock-summary-description');
                            if (summary && !summary.textContent.includes(icon)) {
                                summary.textContent = icon + ' ' + summary.textContent;
                            }
                        }
                    }
                }
            });
        }

        // Add response status enhancement
        function enhanceResponses() {
            const responseHeaders = document.querySelectorAll('.response-col_status');
            responseHeaders.forEach(header => {
                if (!header.dataset.enhanced) {
                    header.dataset.enhanced = 'true';
                    const statusCode = header.textContent.trim();
                    
                    let emoji = '';
                    let color = '';
                    
                    if (statusCode.startsWith('2')) {
                        emoji = '✅';
                        color = '#28a745';
                    } else if (statusCode.startsWith('4')) {
                        emoji = '❌';
                        color = '#dc3545';
                    } else if (statusCode.startsWith('5')) {
                        emoji = '💥';
                        color = '#fd7e14';
                    }
                    
                    if (emoji) {
                        header.innerHTML = `<span style="color: ${color};">${emoji} ${statusCode}</span>`;
                    }
                }
            });
        }

        // Add helpful examples to request bodies
        function addExamples() {
            const tryItButtons = document.querySelectorAll('.try-out__btn');
            tryItButtons.forEach(btn => {
                btn.addEventListener('click', function() {
                    setTimeout(() => {
                        const textareas = document.querySelectorAll('.body-param textarea');
                        textareas.forEach(textarea => {
                            if (!textarea.dataset.exampleAdded && textarea.value === '') {
                                textarea.dataset.exampleAdded = 'true';
                                
                                // Add examples based on context
                                const operationDiv = textarea.closest('.opblock');
                                if (operationDiv) {
                                    const pathSpan = operationDiv.querySelector('.opblock-summary-path span[data-path]');
                                    if (pathSpan) {
                                        const path = pathSpan.textContent;
                                        
                                        if (path.includes('/auth/cliente/login') || path.includes('/clientes/login')) {
                                            textarea.value = JSON.stringify({
                                                "email": "cliente@exemplo.com",
                                                "senha": "senha123"
                                            }, null, 2);
                                        } else if (path.includes('/auth/admin/login')) {
                                            textarea.value = JSON.stringify({
                                                "email": "admin1@petshop.com",
                                                "senha": "admin123"
                                            }, null, 2);
                                        } else if (path.includes('/produtos') && operationDiv.querySelector('.post')) {
                                            textarea.value = JSON.stringify({
                                                "nome": "Ração Premium para Cães",
                                                "descricao": "Ração super premium com ingredientes naturais",
                                                "preco": 89.90,
                                                "categoriaId": 1,
                                                "quantidadeEstoque": 100
                                            }, null, 2);
                                        }
                                    }
                                }
                            }
                        });
                    }, 500);
                });
            });
        }

        // Add keyboard shortcuts info
        function addKeyboardShortcuts() {
            const body = document.body;
            if (!body.querySelector('.keyboard-shortcuts')) {
                const shortcuts = document.createElement('div');
                shortcuts.className = 'keyboard-shortcuts';
                shortcuts.innerHTML = `
                    <div style="position: fixed; bottom: 20px; right: 20px; background: #333; color: white; padding: 10px; border-radius: 8px; font-size: 0.8rem; z-index: 1000; max-width: 200px; opacity: 0.8;">
                        <div style="font-weight: bold; margin-bottom: 5px;">⌨️ Atalhos:</div>
                        <div>Ctrl + / : Buscar</div>
                        <div>Esc : Fechar modal</div>
                        <div>Tab : Navegar</div>
                    </div>
                `;
                body.appendChild(shortcuts);
                
                // Hide after 5 seconds
                setTimeout(() => {
                    shortcuts.style.opacity = '0';
                    setTimeout(() => shortcuts.remove(), 300);
                }, 5000);
            }
        }

        // Performance monitoring
        function addPerformanceInfo() {
            const executeButtons = document.querySelectorAll('.execute');
            executeButtons.forEach(btn => {
                btn.addEventListener('click', function() {
                    const startTime = performance.now();
                    const checkResponse = () => {
                        const responseSection = btn.closest('.opblock').querySelector('.responses-wrapper');
                        if (responseSection && responseSection.querySelector('.response')) {
                            const endTime = performance.now();
                            const duration = Math.round(endTime - startTime);
                            
                            const perfInfo = document.createElement('div');
                            perfInfo.className = 'performance-info';
                            perfInfo.innerHTML = `
                                <div style="background: #f8f9fa; border: 1px solid #dee2e6; border-radius: 4px; padding: 8px; margin: 10px 0; font-size: 0.85rem;">
                                    ⏱️ Tempo de resposta: <strong>${duration}ms</strong>
                                </div>
                            `;
                            
                            const existingPerf = responseSection.querySelector('.performance-info');
                            if (existingPerf) existingPerf.remove();
                            
                            responseSection.appendChild(perfInfo);
                        } else {
                            setTimeout(checkResponse, 100);
                        }
                    };
                    setTimeout(checkResponse, 100);
                });
            });
        }

        // Initialize all enhancements
        function initializeEnhancements() {
            addCustomHeader();
            addAuthTips();
            enhanceOperations();
            enhanceResponses();
            addExamples();
            addKeyboardShortcuts();
            addPerformanceInfo();
        }

        // Run initial setup
        initializeEnhancements();

        // Re-run enhancements when new content is loaded
        const observer = new MutationObserver(function(mutations) {
            mutations.forEach(function(mutation) {
                if (mutation.addedNodes.length > 0) {
                    setTimeout(initializeEnhancements, 500);
                }
            });
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });

        // Add search functionality enhancement
        document.addEventListener('keydown', function(e) {
            if (e.ctrlKey && e.key === '/') {
                e.preventDefault();
                const searchInput = document.querySelector('.filter input');
                if (searchInput) {
                    searchInput.focus();
                    searchInput.select();
                }
            }
        });

        console.log('🐾 PetShop Swagger UI enhancements loaded successfully!');
    });

})();
