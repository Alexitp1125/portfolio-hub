# PROJECT_INSTRUCTIONS — Portfolio Hub

> Fuente de verdad del proyecto. Se lee al inicio de cada sesión y se actualiza al
> cierre con lo que realmente se decidió/hizo. Construir primero, documentar después.
>
> Propietario: Alexi Torres · Dominio: alexitp1125.com · Última actualización: **2026-06-13**

---

## 1. Qué es esto

Plataforma personal tipo **hub / catálogo de apps reales y usables** (no un currículum).
Las aplicaciones son el héroe; la persona (bio, CV, contacto) es secundaria. Cada app
futura vivirá en su propio subdominio, repo y contenedor independiente. El hub solo
muestra el catálogo con enlaces (demo, GitHub, NuGet, descarga). Se crece de forma
iterativa y honesta: lo que no está desplegado no se marca como "live".

---

## 2. Estado REAL del repo (verificado 2026-06-13)

⚠️ **Importante:** este repo está en la etapa **"under construction"**, NO en la etapa
de catálogo que describen los PDFs `Manual_Proyecto_Portfolio.pdf` y
`Guia_Comandos_Portfolio.pdf`. Esos PDFs describen un Dashboard/catálogo que **no
existe en este repo ni en GitHub** (`origin/main` está en el mismo punto). Se tratan
como contexto/referencia del plan, no como descripción del código actual.

Qué hay de verdad hoy:

- Solución Blazor Web App **.NET 10, InteractiveAuto**: proyectos `PortfolioHub`
  (servidor) y `PortfolioHub.Client` (WebAssembly).
- **Única personalización real:** `Components/Pages/Home.razor` reemplazada por una
  página *under construction* estilo terminal (`@rendermode InteractiveServer`, con
  animación de tipeo: campos `_typed` / `_showOutput`).
- **Restos del scaffold por defecto, todavía presentes:**
  - `PortfolioHub.Client/Pages/Counter.razor`
  - `PortfolioHub/Components/Pages/Weather.razor`
  - `NavMenu.razor` con enlaces Home / Counter / Weather (sidebar de plantilla)
  - `MainLayout.razor` con el enlace "About" apuntando a learn.microsoft.com
- `Program.cs` (servidor): estándar del scaffold; registra los dos render modes y
  `AddAdditionalAssemblies(typeof(PortfolioHub.Client._Imports).Assembly)`.
- `PortfolioHub.csproj`: incluye `BlazorDisableThrowNavigationException=true`. **Sin**
  paquetes de EF Core ni Npgsql.
- Bootstrap **local** en `wwwroot/lib/bootstrap` (referenciado en `App.razor`). Aún
  **no** se cargan Bootstrap Icons.
- `Dockerfile` multi-stage presente en la raíz.

Qué **NO** existe todavía (a pesar de lo que digan los PDFs/notas viejas):
catálogo `Dashboard.razor`, `ProjectCard.razor`, carpeta `Models/`, proyecto de datos
`PortfolioHub.Data`, localización ES/EN (`.resx`), páginas About/Contact, footer
propio, SEO (sitemap/robots), base de datos.

**Producción:** los PDFs afirman que `https://alexitp1125.com` está vivo en un Droplet
de DigitalOcean. El contenido realmente desplegado **no se verificó** en esta sesión.
Lo que vive en este repo + GitHub es la página *under construction*.

Historial git actual: `initial commit` → `add Blazor scaffold with under-construction
page` → `add Dockerfile`.

---

## 3. Cómo trabajamos (acuerdo)

- **Paso a paso, cosa por cosa.** Alexi está aprendiendo; explicar el *porqué* de cada
  paso, sobre todo en infra/DevOps. Avanzar de a poco, verificar, y seguir.
- **No pre-decidir por Alexi.** Si una decisión contradice este doc o los PDFs,
  señalarlo antes de actuar (no asumir).
- **Construir primero, documentar después.** Al cierre de cada sesión, actualizar la
  bitácora (§9) con lo que realmente se hizo.
- **Honestidad en el catálogo:** nada se marca "live" si no está desplegado; badges
  "soon" cuando corresponda.

---

## 4. Stack fijo (no proponer alternativas)

- **Framework:** Blazor Web App .NET 10 LTS, InteractiveAuto global. SSR para
  presentación pura; `.Client` (WASM) solo para componentes que necesitan
  interactividad C# real (clicks, estado, eventos). Regla: presentación pura → proyecto
  servidor; interactivo → `.Client`. **No** borrar `_Imports.razor` del Client.
- **UI:** Bootstrap + **CSS isolation por componente** (`Componente.razor.css`) + tema
  oscuro. Preferir variables Bootstrap (`--bs-*`) sobre colores hardcodeados. Sin
  frameworks UI de terceros (nada de MudBlazor, etc.).
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
- **Naming .NET:** `PascalCase` clases/métodos, `camelCase` locales, `_camelCase`
  campos privados.
- **CSS:** isolation por componente; nada de CSS global suelto.
- **Commits:** Conventional Commits — `feat:`, `fix:`, `docs:`, `chore:`.
- **Ramas:** `main` = producción · `feature/*` = trabajo en curso (abrir PR aunque se
  trabaje solo).
- **Tests (cuando exista lógica de dominio):** xUnit + FluentAssertions +
  Moq/NSubstitute; objetivo ~60% en esa capa.

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

- `NavBar` — marca **`alexitp1125`** + enlaces localizados (Projects/About/Contact) +
  `LanguageSwitch` (EN/ES) + botón de tema (claro/oscuro). Responsive: hamburguesa
  CSS en móvil.
- `Footer` — GitHub · LinkedIn · email · NuGet (pendiente de construir).

**Página principal = hub** (`Features/Catalog/`), de arriba abajo:

- `Hero` — titular con gancho + botón "Ver proyectos".
- `ProjectGrid` — rejilla de `ProjectCard` (1 por app: miniatura, título, frase, tags,
  badge live/soon, "ver más").
- `AboutTeaser` — bloque breve "sobre mí" que enlaza a `AboutPage`.

**Por app:** `ProjectDetailPage` en ruta `/apps/{slug}` — info larga, capturas, stack y
todos los recursos (demo, GitHub, NuGet, descarga).

**Páginas sueltas:** `AboutPage`, `ContactPage`.

### 7.2 Dirección visual (confirmada 2026-06-13)

Minimalista sobre **tema oscuro**, con un **único acento: degradado azul→rosa
(vaporwave, con medida)**. El acento aparece solo en pocos elementos (titular del Hero,
botón principal, badge "live", enlaces activos); el resto queda plano y con aire, para
que las capturas de las apps destaquen.

**Tema claro y oscuro:** ambos soportados vía `data-bs-theme` en `<html>`, con un botón
de toggle en el NavBar y persistencia en `localStorage` (script en `js/site.js`; init
sin parpadeo en `App.razor`). **Default: oscuro.** El acento azul→rosa es el mismo en
los dos temas; cambian solo el fondo/superficie/texto (paletas en `app.css` bajo
`[data-bs-theme="dark"]` y `[data-bs-theme="light"]`).

Paleta de partida (a afinar; se define como **variables CSS reutilizables**, no
hardcodeada por componente — ver convención §5):

- Fondo `#0f1021` · superficie `#181a2e` · borde `#272a45`
- Texto `#eef0fb` · texto tenue `#9498b3`
- Acento (degradado) `#38bdf8 → #818cf8 → #f472b6`

---

## 8. Infraestructura (datos de referencia, según los PDFs)

| Qué | Valor |
|---|---|
| Dominio | alexitp1125.com (Cloudflare Registrar) |
| IP servidor | 162.243.22.179 |
| Usuario servidor | alexitp (no-root, sudo + llave SSH) |
| SO | Ubuntu 24.04 LTS · 2 GB RAM · NYC2 |
| DNS | Cloudflare: A (@) + wildcard (*) → 162.243.22.179, "DNS only" |
| Repos | portfolio-hub (público, MIT) · server-infra (privado) |
| Ruta servidor | `~/server-infra` (Caddyfile + compose) · `~/projects/portfolio-hub` (código) |
| Ruta PC | `D:\Dev\portfolio-hub` · `D:\Dev\server-infra` |
| Git identity | Alexi Torres · alexitp1125@gmail.com |

---

## 9. Flujo de despliegue (referencia rápida)

1. **PC:** `git add .` → `git commit -m "feat: ..."` → `git push`
2. **Servidor:** `ssh alexitp@162.243.22.179` → `cd ~/projects/portfolio-hub && git pull`
   → `cd ~/server-infra && docker compose up -d --build`
3. **Navegador:** recargar `https://alexitp1125.com` con Ctrl+F5

Probar en local antes de subir: `dotnet build` y `dotnet run` dentro de
`D:\Dev\portfolio-hub`. (El SDK/NuGet corre en tu PC, no en este entorno.)

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

---

## 11. Próximos pasos (propuesta — se elige el siguiente juntos)

Punto de partida: página *under construction*. Orden sugerido, de menor a mayor riesgo
(no es un compromiso; se confirma paso a paso):

1. **Limpiar el scaffold:** quitar `Counter.razor` y `Weather.razor`, y reescribir
   `NavMenu` / `MainLayout` (sin enlaces de plantilla ni el "About" a Microsoft).
2. **Primera versión del catálogo:** página de inicio Dashboard con hero + grid de
   tarjetas Bootstrap, y un componente `ProjectCard` (CSS aislado). Modelo provisional
   en memoria (datos hardcodeados) hasta que exista la DB.
3. **Piezas del MVP (presentación pura):** footer con enlaces reales, página "Sobre
   mí" + hueco para CV (PDF), página de contacto.
4. **SEO básico:** `<title>` + meta description + Open Graph por página, `robots.txt`,
   `sitemap.xml`.
5. **Switch ES/EN** (interactividad + `.resx` + cookie + middleware) — más enredado;
   se hace con calma.
6. **DB del hub** (Postgres en compose + `PortfolioHub.Data` + EF Core `UseNpgsql` +
   primera migración) — mayor riesgo; opcional, no bloquea nada.
