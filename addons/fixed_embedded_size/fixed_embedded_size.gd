@tool
extends EditorPlugin

const FIXED_SIZE_LABEL := "Fixed Size"
const DISABLED_SIZE_LABELS := ["Keep Aspect Ratio", "Stretch to Fit"]
const PROJECT_METADATA_PATH := "res://.godot/editor/project_metadata.cfg"

var _size_menu: PopupMenu

func _enter_tree() -> void:
	_save_fixed_size_mode()
	set_process(true)

func _exit_tree() -> void:
	set_process(false)
	if is_instance_valid(_size_menu):
		_set_alternate_modes_disabled(false)

func _process(_delta: float) -> void:
	if not is_instance_valid(_size_menu):
		_size_menu = _find_size_menu()

	if not is_instance_valid(_size_menu):
		return

	var fixed_index := _find_item_index(FIXED_SIZE_LABEL)
	if fixed_index == -1:
		_size_menu = null
		return

	if not _size_menu.is_item_checked(fixed_index):
		_size_menu.emit_signal("id_pressed", _size_menu.get_item_id(fixed_index))
		_save_fixed_size_mode()

	_set_alternate_modes_disabled(true)

func _find_size_menu() -> PopupMenu:
	for node in get_tree().root.find_children("*", "PopupMenu", true, false):
		var popup := node as PopupMenu
		if _contains_size_modes(popup):
			return popup

	return null

func _contains_size_modes(popup: PopupMenu) -> bool:
	if _find_item_index_in(popup, FIXED_SIZE_LABEL) == -1:
		return false

	for label in DISABLED_SIZE_LABELS:
		if _find_item_index_in(popup, label) == -1:
			return false

	return true

func _find_item_index(label: String) -> int:
	return _find_item_index_in(_size_menu, label)

func _find_item_index_in(popup: PopupMenu, label: String) -> int:
	for index in range(popup.item_count):
		if popup.get_item_text(index) == label:
			return index

	return -1

func _set_alternate_modes_disabled(disabled: bool) -> void:
	for label in DISABLED_SIZE_LABELS:
		var index := _find_item_index(label)
		if index != -1:
			_size_menu.set_item_disabled(index, disabled)

func _save_fixed_size_mode() -> void:
	var metadata_directory := ProjectSettings.globalize_path("res://.godot/editor")
	var directory_error := DirAccess.make_dir_recursive_absolute(metadata_directory)
	if directory_error != OK:
		push_error("Could not create Godot project editor metadata directory: %s" % error_string(directory_error))
		return

	var config := ConfigFile.new()
	var load_error := config.load(PROJECT_METADATA_PATH)
	if load_error != OK and load_error != ERR_FILE_NOT_FOUND:
		push_error("Could not load Godot project editor metadata: %s" % error_string(load_error))
		return

	config.set_value("game_view", "embed_size_mode", 0)
	var save_error := config.save(PROJECT_METADATA_PATH)
	if save_error != OK:
		push_error("Could not save Godot project editor metadata: %s" % error_string(save_error))
