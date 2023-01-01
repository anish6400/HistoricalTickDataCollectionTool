## Purpose

Ninjatrader is able to get the historical data from the connected exchange for the period of 1 year. However ninjatrader only supports C#. This tool automates the process of turning historical data stored by ninjatrader to txt files. This way people will have the flexibility to perform statistical analysis on historical tick data using other languages like Python, Java, R or any programming language they are more comfortable with.

## Setup

1. Download this repository as a zip file.
2. Paste the extracted folder inside directory: {Your Documents Folder}\Ninjatrader 8\Cutom\AddOns\
3. Launch Ninjatraer
4. Launch Ninjascript Editor. New > Ninjascript Editor
5. Right Click empty space of Ninjascript Editor and click References. Add the dll files in dlls folder as references.
6. Compile the project using the compile button in the top bar of the editor window.
7. Go back to Ninjatrader and launch this tool from Tools > HistoricalTickDataCollectionTool

## Screenshot

![img](https://i.imgur.com/2IKcXCa.png)

## Usage

1. Make sure you have already downloaded the required data using Ninjatrader's builtin Historical Data tool.
2. Launch the tool using from Tools > HistoricalTickDataCollectionTool.
3. In the left panel, select the contracts you want to get the data for. (currently only supports ES, NQ, ZB, ZN)
4. Click "Start Collection" button.

After the collection process is completed the txt file would be organized under this folder: {Your Documents Folder}\HistoricalTickData.

## Format

The structure of tick data inside those txt files should look like this:

| Time in milliseconds     | Bid     | Ask    | Price  | Volume |
| ------------------------ | ------- | ------ | ------ | ------ |
| 04 Jan 2022 15:00:00.016 | 4783.25 | 4783.5 | 4783.5 | 1      |
