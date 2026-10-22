# PROJECT_INSTRUCTIONS — Portfolio Hub

> Fuente de verdad del proyecto. Se lee al inicio de cada sesión y se actualiza al
> cierre con lo que realmente se decidió/hizo. Construir primero, documentar después.
>
> Propietario: Alexi Torres · Dominio: alexitp1125.com · Última actualización: **2026-10-20**
>
> Sigue [`dev-conventions`](https://github.com/Alexitp1125/dev-conventions) (`CONVENTIONS.md` y
> `WORKFLOW.md`). Aquí solo va lo propio del hub: su estado, sus decisiones y su bitácora.

---

## 0. Resumen rápido (leer primero)

- **Dónde estoy:** en producción está el **estilo v3** (commit `766c94e`). En el repo,
  además, el hub es **instalable como app** (`dev-conventions` §11) y las tarjetas de
  **Mixtape** y **Chartwave** están *Live*.
- **Lo último (2026-10-21):** §11 aplicada al hub; Mixtape y Chartwave *Live*.
  **Pendiente de compilar, revisar y desplegar.**
- **Lo siguiente:** desplegar y probar la instalación (Android, iPhone y PC); después, las
  ideas de tarjeta (§11).

---

## 1. Qué es esto

Plataforma personal tipo **hub / catálogo de apps reales y usables** (no un currículum).
Las aplicaciones son el héroe; la persona (bio, CV, contacto) es secundaria. Cada app
futura vivirá en su propio subdominio, repo y contenedor independiente. El hub solo
muestra el catálogo con enlaces (demo, GitHub, NuGet, descarga). Se crece de forma
iterativa y honesta: lo que no está desplegado no se marca como "live".

**El hub es una app más.** Su función es listar las demás; no sabe nada de cómo
funciona cada una por dentro.

---

## 2. Estado REAL del repo (verificado 2026-10-20)

**Producción:** `alexitp1125.com` sirve el commit `766c94e` (estilo v3), desplegado el
2026-10-20. `www.alexitp1125.com` redirige al dominio principal desde el
2026-10-19 (configurado en `server-infra`).

Qué hay:

- Solución Blazor Web App **.NET 10, InteractiveAuto**: `PortfolioHub` (servidor) y
  `PortfolioHub.Client` (WebAssembly).
- **Layout:** `NavBar` (marca `alexitp1125`, enlaces Projects/About/Contact, GitHub,
  `LanguageSwitch` EN/ES, botón de tema, hamburguesa CSS en móvil), `MainLayout` de barra
  superior y `ReconnectModal`.
- **Catálogo** (`Features/Catalog`): `CatalogPage` en `/`, `ProjectCard` (número de orden,
  modo solo texto y fragmento `Media` para el dibujo SVG) y el modelo `Project` +
  `ProjectStatus` (`Live`/`Soon`). Datos en memoria. Dos tarjetas *Live*: **Mixtape**
  (`mixtape.alexitp1125.com`, reproductor de música) y **Chartwave**
  (`chartwave.alexitp1125.com`, generador de gráficas 3D; la v1 es el donut). Mixtape ya es
  instalable (§11, su commit `f6302b0`); Chartwave lo será en su v1.1. Una tarjeta *Live* lleva
  el botón **«Open app»** y,
  si la app es instalable (`Installable`), el botón `<install>` del navegador. Al final,
  la tarjeta **Next app** con Rako.
- **Hero** en `CatalogPage`: nombre y rol, titular con el degradado, frase sobre la
  arquitectura y botones *View projects* / *GitHub*, en un cuadro translúcido sobre la
  ciudad (`HeroCity`).
- **404** (`Pages/NotFound.razor`): Rako con los ojos `><`, texto EN/ES y botón para
  volver.
- **Marca:** colores y fuentes de `wwwroot/brand/tokens.css` (copia del de
  `dev-conventions`); Rako en `wwwroot/brand/` y favicons en `wwwroot/`.
- **Instalable** (`dev-conventions` §11): `manifest.webmanifest`, iconos (`favicon-192/512`,
  `icon-maskable-512.png`, `apple-touch-icon.png`), `sw.js` + `offline.html` y
  `js/app-shell.js`, copiados de `dev-conventions/pwa/`. En modo instalada, la barra muestra
  Atrás, Compartir y Recargar; el botón Instalar sale cuando el navegador puede instalar.
- Localización EN/ES: `IStringLocalizer<SharedResource>` + `Resources/SharedResource*.resx`
  + cookie vía `/culture/set`. Inglés por defecto.
- Tema claro/oscuro con toggle (`js/site.js`, default oscuro).
- **About y Contact son placeholders** ("Under construction").
- `Dockerfile` multi-stage en la raíz.

Qué **no** existe todavía: footer, base de datos, SEO, página de detalle por app,
proyectos de capa (`Data`/`Application`/`Tests`).

Los PDFs de junio (`Manual_Proyecto_Portfolio.pdf`, `Guia_Comandos_Portfolio.pdf`) son
contexto histórico, no el estado actual.

---

## 3. Cómo trabajamos (acuerdo)

- **Paso a paso, cosa por cosa.** Alexi está aprendiendo; explicar el *porqué* de cada
  paso, sobre todo en infra/DevOps. Avanzar de a poco, verificar, y seguir.
- **No pre-decidir por Alexi.** Si una decisión contradice este doc o los PDFs,
  señalarlo antes de actuar (no asumir).
- **Construir primero, documentar después.** Al cierre de cada sesión, actualizar la
  bitácora (§10) con lo que realmente se hizo.
- **Honestidad en el catálogo:** nada se marca "live" si no está desplegado; badges
  "soon" cuando corresponda.
- **Ritmo de trabajo (acordado 2026-10-20):**
  1. Idea → tarjeta temporal en el hub (*Coming soon* + una línea de qué es).
  2. Cada app se desarrolla en su repo, su contenedor y su chat. El hub no se toca.
  3. Cuando la app tiene forma → se vuelve al hub y se actualiza su tarjeta (estado,
     enlaces, captura).
  4. Con 3 apps reales → se trabaja el resto del hub (About, Contact, footer, SEO).
- **El repo es la fuente de verdad, no el chat.** Al cerrar cada sesión, commit de código
  y docs juntos. (Se perdió una cuenta de Claude con todo su historial; lo que estaba en
  el repo sobrevivió.)
- **Se edita en el PC, nunca en el servidor.** En el servidor solo `git pull`.

---

## 4. Stack fijo (no proponer alternativas)

- **Framework:** Blazor Web App .NET 10 LTS, InteractiveAuto global. SSR para
  presentación pura; `.Client` (WASM) solo para componentes que necesitan
  interactividad C# real (clicks, estado, eventos). Regla: presentación pura → proyecto
  servidor; interactivo → `.Client`. **No** borrar `_Imports.razor` del Client.
- **UI:** Bootstrap + **CSS isolation por componente** (`Componente.razor.css`) + tema
  claro y oscuro. Colores y fuentes desde `brand/tokens.css` (en `app.css` se exponen
  como `--ph-*`). Sin frameworks UI de terceros (nada de MudBlazor, etc.).
- **Datos:** PostgreSQL + EF Core, **una DB por servicio** (database-per-service). Las
  DB nunca se exponen a internet (sin subdominio, sin puerto público); acceso desde el
  PC solo por túnel SSH.
- **Interop:** TypeScript cuando haga falta.
- **Arquitectura de referencia:** ApolloWebSolutions (separación datos / lógica /
  visual, organización por feature), adoptada **gradualmente**. Apollo es Blazor
  Server; el hub es Web App → no copiar a ciegas. Crear cada proyecto de capa
  (datos/backend) solo cuando tenga contenido real que lo justifique.

---

## 5. Convenciones

- **Idioma del código:** identificadores, comentarios y nombres de carpeta **siempre en
  inglés** (aunque Alexi y Claude conversen en español).
- **Idioma de la UI:** **inglés es el principal/por defecto**; español es opcional vía
  un switch. Mecanismo: `IStringLocalizer<SharedResource>` + `.resx` en `/Resources`
  (`SharedResource.resx` = inglés/fallback, `SharedResource.es.resx` = español);
  persistencia con cookie de cultura (1 año) vía el endpoint `/culture/set`;
  `UseRequestLocalization` con default `en` y culturas `en`/`es`.
- **Naming .NET:** `PascalCase` en clases, métodos y propiedades; `camelCase` en
  variables locales y campos privados (**sin guion bajo**).
- **CSS:** isolation por componente; nada de CSS global suelto.
- **Commits:** Conventional Commits — `feat:`, `fix:`, `docs:`, `chore:`.
- **Ramas:** `main` = producción · `feature/*` = trabajo en curso (abrir PR aunque se
  trabaje solo).
- **Tests (cuando exista lógica de negocio):** NUnit.

- **Reglas completas:** `dev-conventions/CONVENTIONS.md` (nombres, capas, ViewModels,
  estilo de código). Si algo de esta sección choca con ese archivo, gana ese archivo.

---

## 6. Arquitectura (subdominios independientes)

El hub NO embebe demos: es un punto central con enlaces. Cada proyecto futuro =
1 repo + 1 Dockerfile + 1 contenedor + 1 subdominio + su DB si la necesita. El DNS
wildcard ya cubre subdominios futuros (no se toca Cloudflare al agregar uno). Un solo
Caddy enruta por subdominio y gestiona HTTPS (Let's Encrypt). El hub tendrá su propia
DB (`hubDB`) cuando llegue ese momento.

---

## 7. Estructura del proyecto (decidida 2026-06-13)

Enfoque **gradual**. Por ahora solo se organiza el proyecto Web **por feature**. Los
proyectos de capa (`PortfolioHub.Data`, `PortfolioHub.Application`, `PortfolioHub.Tests`)
se crean más adelante, cuando haya contenido real que los justifique (DB, lógica,
pruebas). El host **no se renombra**: `PortfolioHub` se queda como está (renombrar
tocaría `.sln`, namespaces y el `ENTRYPOINT` del `Dockerfile` sin aportar valor ahora).

Solución hoy: `PortfolioHub` (Web/host, Razor SSR) + `PortfolioHub.Client` (WASM,
interactividad). Dependencia objetivo cuando existan las capas: **Web → Application →
Data** (siempre hacia adentro; nunca al revés).

Organización por feature dentro del Web (objetivo):

```
PortfolioHub/Components/
  Layout/      MainLayout · NavMenu · Footer
  Features/
    Catalog/   CatalogPage (@page "/") · ProjectCard (+ .razor.css)
    About/     AboutPage · enlace al CV (PDF)
    Contact/   ContactPage
  Shared/      piezas de UI reutilizables
```

Regla: cada feature agrupa su página y sus componentes juntos (no carpetas por tipo).
Los datos del catálogo van en memoria (hardcodeados) hasta montar la DB.

### 7.1 Estructura de la UI (componentes)

Prioridad de la página: (1) las apps, (2) los recursos de cada app, (3) la persona.

Componentes **persistentes** (en `Layout/`, en todas las páginas):

- `NavBar` — **hecho.** Marca **`alexitp1125`** + enlaces localizados (Projects/About/
  Contact) + `LanguageSwitch` (EN/ES) + botón de tema. Hamburguesa CSS en móvil.
- `Footer` — GitHub · LinkedIn · email · NuGet. **Pendiente** (cuando haya 3 apps).

**Página principal = hub** (`Features/Catalog/`), de arriba abajo:

- `Hero` — **hecho 2026-10-20** (dentro de `CatalogPage`): titular con el degradado,
  frase y botones *View projects* / *GitHub*, sobre la ciudad (`HeroCity`).
- `ProjectGrid` — **hecho** (dentro de `CatalogPage`): rejilla de `ProjectCard` + la
  tarjeta *Next app*.
- `AboutTeaser` — **pendiente**, cuando exista `AboutPage`.

**Por app:** `ProjectDetailPage` en ruta `/apps/{slug}` — info larga, capturas, stack y
todos los recursos (demo, GitHub, NuGet, descarga). **Pendiente.**

**Páginas sueltas:** `AboutPage`, `ContactPage` (hoy placeholders).

### 7.2 Dirección visual (estilo v3, decidido 2026-10-20)

City-pop / vaporwave **con medida**, en tema claro y oscuro (por defecto, oscuro). La
referencia es la demo `claude/hub-style-demo-v5.html` (documentos del proyecto).

- **Hero:** la ciudad (`HeroCity`) a lo ancho, con el cuadro del titular encima. Sol en el
  tema claro, luna en el oscuro, en su propia capa colocada desde arriba del Hero para que
  no se salgan en ninguna resolución. El Hero crece en pantallas muy anchas para que los
  edificios no tapen el cielo. **La ciudad es provisional:** puede cambiar por otra
  ilustración; solo se toca `HeroCity`.
- **Fondo de página (variante B):** un toque del cielo arriba y rosa suave al final.
- **Paleta:** la v3, en `brand/tokens.css` (`dev-conventions`). Los colores propios de la
  ciudad y de los dibujos de las tarjetas están en sus componentes.
- **Fuentes:** Jost (títulos y texto) y Space Mono (etiquetas), desde Google Fonts.
- **Tarjetas:** borde de tinta, sombra desplazada, dibujo SVG arriba y badge en Space Mono.
- **Rako** como máximo 2 o 3 veces por página: logo de la barra, tarjeta *Next app* y 404
  (más el favicon).
- **Instalada como app** (§11 de `dev-conventions`, demo `claude/hub-pwa-demo.html`): icono
  de Rako sobre «cielo de noche»; Atrás junto al logo, Compartir y Recargar en los controles.

---

## 8. Infraestructura (datos de referencia)

| Qué | Valor |
|---|---|
| Dominio | alexitp1125.com (Cloudflare Registrar) |
| IP servidor | 162.243.22.179 |
| Usuario servidor | alexitp (no-root, sudo + llave SSH) · desde el PC: `ssh hub` |
| SO | Ubuntu 24.04 LTS · 2 GB RAM · NYC2 |
| DNS | Cloudflare: A (@) + wildcard (*) → 162.243.22.179, "DNS only" |
| Repos | portfolio-hub (público, MIT) · server-infra (privado) |
| Ruta servidor | `~/server-infra` (Caddyfile + compose) · `~/projects/portfolio-hub` (código) |
| Ruta PC | `C:\dev\portfolio-hub` · `C:\dev\server-infra` |
| Git identity | Alexi Torres · alexitp1125@gmail.com |

Todo lo de infraestructura (desplegar, añadir una app nueva, cuidados) está en el
**README de `server-infra`**.

---

## 9. Flujo de despliegue (referencia rápida)

1. **PC, probar en local:** `cd C:\dev\portfolio-hub` → `dotnet run --project PortfolioHub\PortfolioHub`
2. **PC, subir:** `git add .` → `git commit -m "feat: ..."` → `git push`
3. **Servidor:** `ssh hub` → `cd ~/projects/portfolio-hub && git pull`
   → `cd ~/server-infra && docker compose up -d --build hub`
4. **Navegador:** recargar `https://alexitp1125.com` con Ctrl+F5

---

## 10. Bitácora de sesiones

- **2026-06-13** — Sesión de re-sincronización. Se descubrió que el repo estaba en
  *under construction* (no en la etapa de catálogo de los PDFs); Alexi confirmó que
  había bajado un proyecto y probado el modelo Fable. Acuerdo: seguir desde *under
  construction*, cosa por cosa. Se creó este `PROJECT_INSTRUCTIONS.md` como fuente de
  verdad real. (Sin cambios de código todavía.)
  - Se planificó la **estructura del proyecto** (§7): enfoque gradual, organización por
    feature, capas Data/Application/Tests diferidas, host sin renombrar.
  - **Limpieza del scaffold:** borrados `Counter.razor` (.Client) y `Weather.razor`
    (servidor); quitado `using PortfolioHub.Client.Pages;` de `Program.cs`; `NavMenu`
    sin enlaces Counter/Weather (solo Home); `MainLayout` sin el enlace "About" de
    plantilla. Creadas las carpetas `Components/Features/{Catalog,About,Contact}` y
    `Components/Shared` (con `.gitkeep`). Alexi confirmó que `dotnet build`/`dotnet run`
    pasaron OK.
  - Se diseñó la **estructura de la UI** (§7.1) y se confirmó la **dirección visual**
    (§7.2): minimalista oscuro + acento degradado azul→rosa (vaporwave con medida).
  - **Paso 1 construido (base visual + NavBar):** `data-bs-theme="dark"` en `App.razor`;
    paleta como variables CSS (`--ph-*`) en `app.css` + utilidades `.ph-page`/`.ph-muted`;
    nuevo `Layout/NavBar.razor` (+css, marca + enlaces, subrayado de acento en activo);
    `MainLayout` reescrito a layout de barra superior (shell vertical); borrado `NavMenu`.
    Creados stubs `Features/About/AboutPage.razor` (/about) y
    `Features/Contact/ContactPage.razor` (/contact) para que el NavBar no dé 404.
  - **Paso 2 (idioma + tema + NavBar responsive):** localización EN/ES con
    `IStringLocalizer` + resx + endpoint `/culture/set` (inglés por defecto); textos de
    UI pasados a inglés como base; tema claro/oscuro con toggle (`site.js`, default
    oscuro); NavBar reorganizado (marca `alexitp1125`, enlaces localizados,
    `LanguageSwitch`, botón de tema) y responsive con hamburguesa CSS; Bootstrap Icons
    por CDN.
  - **Fixes tras prueba de Alexi:** (1) NavBar salía centrado porque la clase `.nav`
    chocaba con Bootstrap (`.nav` lo hacía flex y `margin:auto` encogía/centraba el
    contenido) → todas las clases del NavBar/MainLayout pasaron a prefijo `ph-`. (2) El
    tema no persistía al cambiar de página porque la navegación mejorada de Blazor
    resetea atributos de `<html>` → `site.js` reaplica el tema guardado en el evento
    `enhancedload`. El idioma persiste por cookie (no requería fix). Lección: **prefijar
    siempre las clases propias con `ph-` para no chocar con Bootstrap.**
    Pendiente: Alexi corre `dotnet build`/`run`, revisa, y luego commit + deploy
    (objetivo antes del lunes).

- **2026-10-14** — La PC que tenía la llave SSH se rompió. Acceso recuperado: llave
  ed25519 nueva (`id_ed25519_hub`) autorizada desde la consola de DigitalOcean (reset de
  root), alias `ssh hub`, llave vieja eliminada. Servidor actualizado (`apt upgrade`,
  Docker al día). Verificado: el servidor estaba en `6fce62d`, igual que GitHub; no se
  perdió nada.
- **2026-10-19** — PC nueva: Git, Visual Studio 2026 (workload ASP.NET), repos clonados en
  `C:\dev`. En `server-infra`: redirección `www`, cabeceras de seguridad y compresión
  compartidas, límite de logs de Docker, HTTP/3 y README. Verificado: SSH no acepta
  contraseñas y las actualizaciones de seguridad son automáticas. Pendiente: cerrar el
  login de root por SSH (anotado en el README de `server-infra`).
- **2026-10-20** — Repaso del hub. Acordado el ritmo de trabajo (§3) y que el foco es la
  página principal; el resto de pestañas espera a tener 3 apps. Hero escrito en
  `CatalogPage` (EN/ES) a partir de una demo aprobada. Sin compilar todavía (Claude no
  tiene el SDK de .NET en su entorno).
- **2026-10-20 (2)** — Hero compilado, commiteado (`6386d8f`, `90ad51b`) y desplegado.
  El hub pasa a seguir `dev-conventions`: `CLAUDE.md` nuevo que importa `CONVENTIONS.md`,
  `WORKFLOW.md` y este archivo; §5 con `camelCase` sin guion bajo y NUnit; `_donut` →
  `Donut` (es una propiedad, así que va en `PascalCase`); namespaces en bloque en
  `Project.cs` y `SharedResource.cs`. Sin cambios visuales.
- **2026-10-20 (3)** — Estilo v3 llevado al código, a partir de las demos A/B, v4 y v5.
  Variante **B** (degradado suave en toda la página). La **paleta v3** pasa a ser la
  oficial: `tokens.css` en `dev-conventions` (con su entrada en `DECISIONS.md`), copiado a
  `wwwroot/brand/`. Nuevo `HeroCity` (la ciudad en SVG, con sol y luna en su propia capa),
  tarjetas con el estilo de la demo, tarjeta *Next app* y 404 con Rako, favicon de Rako y
  fuentes Jost y Space Mono. Las tarjetas ahora son **Music Player** (antes Lyrics: será un
  reproductor, por legalidad) y **3D Charts** (antes 3D Donut Chart: gráficas en general).
  Nombres provisionales. Comprobado con una maqueta estática (1024–2560 px y móvil),
  compilado y revisado en local por Alexi, y desplegado (`26253b5` y `766c94e`). El
  `favicon.png` de la plantilla se borra en el commit de cierre.
- **2026-10-21** — Nueva regla de plataforma: toda app se puede instalar en el teléfono y
  en el PC (`dev-conventions` §11, escrita por el chat de configuración a partir de
  `claude/pwa-brief.md` y la demo `claude/hub-pwa-demo.html`). Aplicada al hub: manifiesto,
  iconos opacos (el `favicon-180.png` transparente salía negro en iPhone y se borra),
  service worker con página sin conexión, `app-shell.js`, barra con Atrás, Compartir,
  Recargar e Instalar (+ pasos de iPhone) y zonas seguras. Las tarjetas pasan a *Live*:
  Music Player es ahora **Mixtape** (instalable, `Installable = true`; su frase sale de su
  manifiesto) y 3D Charts es **Chartwave** (repo `chartwave`, antes `graphic-generator`; será
  instalable en su v1.1). «Open app» pasa a ser un botón. `Project.DemoUrl` pasa a `AppUrl` y
  aparece `Installable`. Alexi instaló el hub con el botón Instalar en Edge (PC).

---

## 11. Próximos pasos

1. **Cerrar §11:** `dotnet build`/`run`, revisar, `git rm wwwroot/favicon-180.png`, commit y
   desplegar. Probar la instalación en Chrome y Brave (Android), Safari (iPhone) y Chrome o
   Edge (PC): Atrás, Compartir, Recargar, sin conexión y zonas seguras.
2. **Tarjetas:**
   - **Chartwave:** `Installable = true` cuando salga su v1.1 (§11). Si el repo es público,
     añadir `GitHubUrl`.
   - Con **Chrome/Edge 158** (17-nov-2026): probar el botón de instalar de la tarjeta de
     Mixtape en el PC y ver si la app tiene que autorizar al hub (`install_sources`).
3. **Ideas para la tarjeta, por decidir:** etapas en vez de Live/Soon (*Idea → Prototipo
   → En desarrollo → Live*); el subdominio a la vista cuando esté *Live*; una línea de
   "lo interesante" técnico de cada app.
4. **README público** del repo: es lo primero que ve un reclutador en GitHub.
5. Con 3 apps: About, Contact, footer, SEO.
6. Más adelante: DB del hub; CI/CD con GitHub Actions (el CV ya lo menciona).
