# Clear Scope

SPT 4.x port of peinwastaken's MIT-licensed scope eye relief tweak.

Original mod/code by peinwastaken.
SPT 4.x port by ximilleyt.

The mod patches `EFT.CameraControl.OpticSight.Awake` and `OpticSight.OnEnable`,
then increases the `_Scales.x` and `_Scales.y` values on optic lens materials.
Higher values reduce scope shadow/eye relief limits.

## Build

Set `TarkovDir` to your SPT install directory:

```powershell
dotnet build .\ClearScope.sln -c Release /p:TarkovDir="C:\Path\To\SPT\"
```

Alternatively, copy `ClearScope.csproj.user.example` to `ClearScope.csproj.user`
and set `TarkovDir` there.

Copy `ClearScope.dll` from `bin\Release\` to:

```text
<SPT>\BepInEx\plugins\
```

## Configuration

BepInEx will create `com.pein.clearscope.cfg` after the game starts.

- `Enabled`: global toggle.
- `Scale X Multiplier`: default `1.4`, allowed range `0.1` to `5`.
- `Scale Y Multiplier`: default `1.4`, allowed range `0.1` to `5`.
- `Diagnostic Logging`: default `false`.

## PiP-Disabler

PiP-Disabler can render its own scope vignette on top of the vanilla optic
material. If black edges remain while using PiP-Disabler, set these values in
`BepInEx\config\com.fiodor.pipdisabler.cfg`:

```ini
[Scope Effects]
Vignette = false
Vignette Opacity = 0
```
