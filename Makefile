ROOT_DIR := $(dir $(realpath $(lastword $(MAKEFILE_LIST))))
OUTPUTS  := \
	-name "bin" -or \
	-name "obj"

.PHONY: all

# General use

all: all-online

all-online:
	$(MAKE) -C tools invoke-build

dbg:
	$(MAKE) -C tools invoke-build ENVIRONMENT=Debug

doc:
	$(MAKE) -C tools invoke-doc-build

clean:
	find $(ROOT_DIR) -type d \( $(OUTPUTS) \) -print -exec rm -rf {} +

# This makefile is just a wrapper for tools scripts.
