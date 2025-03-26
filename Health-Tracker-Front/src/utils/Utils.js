export const formatValue = (value) => {
  // Use regular formatting without compact notation or unnecessary decimals
  const numberFormat = new Intl.NumberFormat('en-US');

  if (value >= 1000000) {
    return numberFormat.format(value); // No compact notation, show full value
  } else if (value >= 1000) {
    return numberFormat.format(value); // No compact notation, show full value
  } else {
    return numberFormat.format(value);
  }
};

export const formatThousands = (value) => Intl.NumberFormat('en-US', {
  maximumSignificantDigits: 3,
  notation: 'compact',
}).format(value);

export const getCssVariable = (variable) => {
  return getComputedStyle(document.documentElement).getPropertyValue(variable).trim();
};

const adjustHexOpacity = (hexColor, opacity) => {
  hexColor = hexColor.replace('#', '');

  const r = parseInt(hexColor.substring(0, 2), 16);
  const g = parseInt(hexColor.substring(2, 4), 16);
  const b = parseInt(hexColor.substring(4, 6), 16);

  return `rgba(${r}, ${g}, ${b}, ${opacity})`;
};

const adjustHSLOpacity = (hslColor, opacity) => {
  return hslColor.replace('hsl(', 'hsla(').replace(')', `, ${opacity})`);
};

const adjustOKLCHOpacity = (oklchColor, opacity) => {
  return oklchColor.replace(/oklch\((.*?)\)/, (match, p1) => `oklch(${p1} / ${opacity})`);
};

export const adjustColorOpacity = (color, opacity) => {
  if (color.startsWith('#')) {
    return adjustHexOpacity(color, opacity);
  } else if (color.startsWith('hsl')) {
    return adjustHSLOpacity(color, opacity);
  } else if (color.startsWith('oklch')) {
    return adjustOKLCHOpacity(color, opacity);
  } else {
    throw new Error('Unsupported color format');
  }
};

export const oklchToRGBA = (oklchColor) => {
  const tempDiv = document.createElement('div');
  tempDiv.style.color = oklchColor;
  document.body.appendChild(tempDiv);

  const computedColor = window.getComputedStyle(tempDiv).color;
  document.body.removeChild(tempDiv);

  return computedColor;
};
