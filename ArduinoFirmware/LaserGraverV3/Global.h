#ifndef GLOBALH
#define GLOBALH

#include "Line.h"
#include "Config.h"

static Line lineX (/*size*/X_SIZE, /*STEP pin*/X_STP_PIN, /*DIR pin*/X_DIR_PIN, /*EN pin*/X_EN_PIN, /*Invert direction*/false, /*Endstop pin (analog)*/X_END_APIN);
static Line lineY (/*size*/Y_SIZE, /*STEP pin*/Y_STP_PIN, /*DIR pin*/Y_DIR_PIN, /*EN pin*/Y_EN_PIN, /*Invert direction*/false, /*Endstop pin (analog)*/Y_END_APIN);

#endif
