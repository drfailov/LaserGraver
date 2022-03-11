#ifndef GLOBALH
#define GLOBALH

#include "Line.h"
#include "Config.h"

static Line lineX (X_SIZE, X_STP_PIN, X_DIR_PIN, X_EN_PIN, false, X_END_APIN);
static Line lineY (Y_SIZE, Y_STP_PIN, Y_DIR_PIN, Y_EN_PIN, false, Y_END_APIN);

#endif
