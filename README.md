# RAWPhotosSorter

### Context menu installation
To help your work, you can add this software into the Windows explorer context menu :
To do so, you just need to add entries into Windows Registry.

Under :
<pre><code>
-- HKEY_CLASSES_ROOT/
   |-- Directory/
      |-- Background/
          |-- shell/
            |-- RAWPhotosSorter
                (par défaut)    "Trier pour Lightroom"
                Icon            (exe file location)
                |-- Command
                    (par défaut)    "(exe file location)" "%V"
</code></pre>

### Help keystrokes

|Key|What id does|
|---|:-----------|
|Arrow keys|Navigate through photos|
|S key|Send current photo into destination folder|
|D key|Open destination folder in explorer|
|P key|Open Settings window|
|H key|Open Help windows|
